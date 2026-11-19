# Azure deployment

Funk Workbench reuses an existing Azure Container Registry, such as GramParser's,
with a separate `funk-workbench` image repository. The Bicep template deploys the
Container Apps environment and Container App. It uses existing managed identities
and does not create or change the registry.

**Continuous Build / Test** builds and tests the solution on pushes, then builds
and publishes the container image for `main`. It publishes a unique tag and updates
`latest` only if the build still matches main (ignoring documentation-only changes).
It can also be started manually. It does not deploy Azure resources or app revisions.

**Deploy Workbench to Azure** runs only on manual request (`workflow_dispatch`) on
`main`. It does not build, test, or publish application images. It resolves the
already-published `funk-workbench:latest` to an immutable digest, then deploys the
infrastructure and that digest, checking revision readiness and the HTTPS URL.
Both workflows use GitHub's `prod` environment and share a concurrency group.

Set `CONTAINER_APP_ENVIRONMENT_NAME` independently of the app name, for example
`funk-prod-env`. Bicep creates or updates this managed environment in `RESOURCE_GROUP`;
future apps can reference it without tying its name to the Workbench. Keep one
infrastructure owner for the shared environment's configuration. Changing an
existing app's environment requires app recreation/migration; changing the variable
is not an in-place move and incremental deployment does not delete the old environment.

## 1. Set up Azure prerequisites

Create the Funk resource group and use an existing registry in the same
subscription; the registry can be in another resource group. Ensure the
`Microsoft.App` resource provider is registered. Create or reuse three
user-assigned managed identities and grant these roles before running the workflows:

| Identity | Scope | Roles |
| --- | --- | --- |
| Publishing (`ACR_CLIENT_ID`) | Registry | Reader and `AcrPush` |
| Deployment (`INFRA_CLIENT_ID`) | Funk resource group; registry | Contributor on the resource group; Reader on the registry |
| Runtime (`ACR_PULL_IDENTITY_NAME`) | Registry | `AcrPull`; the identity itself must be in the Funk resource group |

If the registry uses **RBAC Registry + ABAC Repository Permissions**, use
**Container Registry Repository Writer** instead of `AcrPush` and
**Container Registry Repository Reader** instead of `AcrPull`. These roles can
be scoped to `funk-workbench` with an ABAC condition. Keep the Azure Reader
assignments for registry lookup, and don't change the shared registry's mode.

The registry must be reachable by GitHub-hosted runners and Container Apps, and
must allow ARM-audience tokens for managed-identity image pulls. Existing registry
firewall or private-endpoint restrictions require a network-specific runner and
Container Apps networking setup, outside this template's scope.

In Azure Portal, open **Federated credentials** on both the publishing and
deployment identities (not the runtime pull identity). Select the **GitHub
Actions deploying Azure resources** scenario and use these values. The Funk
repository uses GitHub's immutable OIDC subject format.

| Field | Value |
| --- | --- |
| Issuer URL | `https://token.actions.githubusercontent.com` |
| Organization / Organization ID | `vplauzon` / `10082137` |
| Repository / Repository ID | `funk` / `987274380` |
| Entity / Environment | **Environment** / `prod` |
| Subject identifier | `repo:vplauzon@10082137/funk@987274380:environment:prod` |
| Name | `github-funk-prod` (unique within each identity) |
| Audience | `api://AzureADTokenExchange` |

The GitHub environment name is independent of `CONTAINER_APP_ENVIRONMENT_NAME`.
Allow several minutes for Azure role assignments and federation to propagate.

## 2. Configure GitHub

In **Settings > Environments**, create `prod`. Restrict its deployment branches to
`main`; optionally require approval. If previously configured under `dev`, move or
recreate the variables and secrets under `prod`. Configure these environment
variables:

| Variable | Value |
| --- | --- |
| `RESOURCE_GROUP` | Existing Funk resource group name |
| `ACR_NAME` | Existing registry name, without `.azurecr.io` |
| `CONTAINER_APP_NAME` | Container App name, e.g. `funk-workbench` |
| `CONTAINER_APP_ENVIRONMENT_NAME` | Container Apps environment name, e.g. `funk-prod-env` |
| `ACR_PULL_IDENTITY_NAME` | Existing runtime pull identity name |

Configure these environment secrets (all are identifiers, not client passwords):

| Secret | Value |
| --- | --- |
| `ACR_CLIENT_ID` | Publishing managed identity's client ID |
| `INFRA_CLIENT_ID` | Deployment managed identity's client ID |
| `AZURE_TENANT_ID` | Azure tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |

## 3. Deploy

Merge these files into `main`. Wait for **Continuous Build / Test** to finish
publishing `funk-workbench:latest`, or run that workflow manually on `main`.
Then select **Actions > Deploy Workbench to Azure > Run workflow** on `main`.
The HTTPS address appears in the deployment summary. An empty registry is fine
for the build workflow, but deployment requires a successful image publication first.
Pushes publish images but do not deploy; run the deploy workflow for each release.
The deploy workflow uses `ACR_CLIENT_ID` only to read the published digest, then
switches to `INFRA_CLIENT_ID` for infrastructure deployment. The runtime pull
identity still needs to exist and have registry pull permissions before deployment.

The app runs as a non-root user on HTTP port 8080 inside the container. Azure
terminates HTTPS, supplies the public certificate, and redirects HTTP to HTTPS.
Startup and readiness probes request the existing `/` page. The app is public and
does not add authentication; it currently serves the static Workbench described in
[the specs](../code/Funk.Workbench/Specs/README.md).

The Consumption app uses 0.25 vCPU, 0.5 GiB RAM, 0-1 replicas, and a five-minute
scale-down cooldown. Expect a cold start after idle periods. No additional registry
is created, so the existing registry's base fee is shared; Funk images consume its
storage. Logs can be streamed with Azure CLI but are not retained in Log Analytics:

```powershell
az containerapp logs show --resource-group '<funk-resource-group>' --name '<app-name>' --follow
```

Images are retained; there is no automatic registry cleanup. Failed deployments
leave the published image available for investigation. Review registry storage
periodically. Deployments use ARM incremental mode and do not remove resources
that are no longer in the templates. To retire Funk, remove its registry role
assignments and image repository separately. Do not delete the managed environment
or its resource group while another app uses it. Delete the resource group only
after confirming it contains no resources to keep. Never delete the shared
registry or its resource group as part of Funk cleanup.

## Local checks

```powershell
az bicep build --file deployment\funk-infra.bicep --stdout > $null
dotnet build code\FunkSolution.sln --configuration Release
dotnet test code\FunkSolution.sln --configuration Release --no-build
docker build --file deployment\Dockerfile --tag funk-workbench:local code
docker run --rm --publish 8080:8080 funk-workbench:local
```

Open `http://localhost:8080` for the local container. To inspect Azure changes before
a deployment, use `az deployment group what-if` with `deployment\funk-infra.bicep`
and the same parameters as the workflow, including a published image digest.

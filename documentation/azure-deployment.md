# Azure deployment

Funk Workbench reuses an existing Azure Container Registry, such as GramParser's,
with a separate `funk-workbench` image repository. Funk gets its own three
user-assigned managed identities, Container Apps environment, and Container App.
Bicep compiles to ARM templates; no application code changes are needed. Neither
template creates a registry or changes its configuration, pricing tier, or images.

**Continuous Build / Test** builds and tests the solution on pushes, then builds
and publishes the container image for `main`. It publishes a unique tag and updates
`latest` only if the build still matches main (ignoring documentation-only changes).
It can also be started manually. It does not deploy Azure resources or app revisions.

**Deploy Workbench to Azure** runs only on manual request (`workflow_dispatch`) on
`main`. It does not build, test, or publish application images. It resolves the
already-published `funk-workbench:latest` to an immutable digest, then deploys the
infrastructure and that digest, checking revision readiness and the HTTPS URL.
Both workflows use GitHub's `dev` environment and share a concurrency group.

Set `CONTAINER_APP_ENVIRONMENT_NAME` independently of the app name, for example
`funk-dev-env`. Bicep creates or updates this managed environment in `RESOURCE_GROUP`;
future apps can reference it without tying its name to the Workbench. Keep one
infrastructure owner for the shared environment's configuration. Changing an
existing app's environment requires app recreation/migration; changing the variable
is not an in-place move and incremental deployment does not delete the old environment.

## 1. Bootstrap Azure once

Use a dedicated Funk resource group and an existing registry in the same
subscription; the registry can be in another resource group. Bootstrap requires
resource deployment permissions on both resource groups, plus role-assignment
permissions on the Funk resource group and registry (for example, Owner on both
groups). The workflow identity cannot grant itself its initial permissions.

The bootstrap role assignments require the registry's **RBAC Registry Permissions**
mode, not **RBAC Registry + ABAC Repository Permissions**. Do not change a shared
registry's mode for Funk. If it uses ABAC, create the identities and assignments
manually instead: use Container Registry Repository Writer for publishing,
Container Registry Repository Reader for pulling, Reader on the registry for both
GitHub identities, and Contributor on the Funk resource group for infrastructure.
The deployment workflow and app template work with either mode.

The registry must be reachable by GitHub-hosted runners and Container Apps, and
must allow ARM-audience tokens for managed-identity image pulls. Existing registry
firewall or private-endpoint restrictions require a network-specific runner and
Container Apps networking setup, outside this template's scope.

Install Azure CLI with Bicep, sign in, and run these PowerShell commands from the
repository root. Choose your subscription and an available Container Apps region,
and supply the name of your existing registry.

```powershell
az login
az account set --subscription '<subscription-id>'

$resourceGroup = 'funk-dev'
$location = 'canadacentral'
$acrName = '<existing-registry-name>'
$acrResourceGroup = az acr show --name $acrName --query resourceGroup --output tsv
if ($LASTEXITCODE -ne 0) { throw 'Could not resolve the existing registry.' }
$roleMode = az acr show --name $acrName --query roleAssignmentMode --output tsv
if ($LASTEXITCODE -ne 0) { throw 'Could not read the registry role mode.' }
if ($roleMode -eq 'AbacRepositoryPermissions') {
  throw 'Use the manual ABAC identity setup described above; do not change the registry mode.'
}
$appName = 'funk-workbench'

az provider register --namespace Microsoft.App --wait
az provider register --namespace Microsoft.ContainerRegistry --wait
az provider register --namespace Microsoft.ManagedIdentity --wait
az group create --name $resourceGroup --location $location
az bicep build --file deployment\bootstrap.bicep --stdout > $null
az deployment group create --name funk-bootstrap `
  --resource-group $resourceGroup `
  --template-file deployment\bootstrap.bicep `
  --parameters acrName=$acrName acrResourceGroupName=$acrResourceGroup appName=$appName `
  --query properties.outputs --output json
```

Keep the bootstrap outputs for the GitHub configuration below. They contain
identifiers, not credentials. The template creates:

| Identity | Permission | Purpose |
| --- | --- | --- |
| `<appName>-publish` | `AcrPush` and Reader on the registry | Resolve the registry and publish images from GitHub |
| `<appName>-infra` | Contributor on the Funk resource group; Reader on the registry | Deploy the app and environment; attach the pull identity |
| `<appName>-pull` | `AcrPull` on the registry | Pull images when Container Apps starts a replica |

Registry roles apply to all repositories in this shared registry; no delete role
is granted. Bootstrap only adds assignments for Funk's new identities and leaves
other consumers' permissions intact. Publishing and infrastructure identities
each trust GitHub OIDC tokens with the exact subject
`repo:vplauzon/funk:environment:dev`. No client secrets or registry passwords are used.
The pull identity has no GitHub federation.

If using a fork or another GitHub environment, pass `githubRepository` and
`githubEnvironment` to bootstrap and update the workflow environment to match.
Allow several minutes for Azure role assignments and federation to propagate.
Bootstrap is repeatable with the same names; it does not deploy or reset the app.
Keep the resource group, location, and names consistent across deployments.

## 2. Configure GitHub

In **Settings > Environments**, create `dev`. Restrict its deployment branches to
`main`; optionally require approval. Configure these environment variables using
the bootstrap output values:

| Variable | Bootstrap output |
| --- | --- |
| `RESOURCE_GROUP` | `resourceGroupName.value` |
| `ACR_NAME` | `registryName.value` |
| `CONTAINER_APP_NAME` | `containerAppName.value` |
| `CONTAINER_APP_ENVIRONMENT_NAME` | Choose independently, e.g. `funk-dev-env` (not a bootstrap output) |
| `ACR_PULL_IDENTITY_NAME` | `pullIdentityName.value` |

Configure these environment secrets (all are identifiers, not client passwords):

| Secret | Bootstrap output |
| --- | --- |
| `ACR_CLIENT_ID` | `publishClientId.value` |
| `INFRA_CLIENT_ID` | `infraClientId.value` |
| `AZURE_TENANT_ID` | `tenantId.value` |
| `AZURE_SUBSCRIPTION_ID` | `subscriptionId.value` |

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
az containerapp logs show --resource-group $resourceGroup --name $appName --follow
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
az bicep build --file deployment\bootstrap.bicep --stdout > $null
az bicep build --file deployment\funk-infra.bicep --stdout > $null
dotnet build code\FunkSolution.sln --configuration Release
dotnet test code\FunkSolution.sln --configuration Release --no-build
docker build --file deployment\Dockerfile --tag funk-workbench:local code
docker run --rm --publish 8080:8080 funk-workbench:local
```

Open `http://localhost:8080` for the local container. To inspect Azure changes before
a deployment, use `az deployment group what-if` with `deployment\funk-infra.bicep`
and the same parameters as the workflow, including a published image digest.

targetScope = 'resourceGroup'

@description('Region for the Funk managed identities.')
param location string = resourceGroup().location

@description('Name of the existing Azure Container Registry.')
param acrName string

@description('Resource group containing the existing registry, in this subscription.')
param acrResourceGroupName string

@description('Container App name, also used to name its dedicated identities.')
@minLength(2)
@maxLength(32)
param appName string = 'funk-workbench'

@description('GitHub repository allowed to deploy, in owner/name form.')
param githubRepository string = 'vplauzon/funk'

@description('GitHub environment used by the deployment workflow.')
param githubEnvironment string = 'prod'

module registryAccess 'registry-access.bicep' = {
  name: '${appName}-registry-access'
  scope: resourceGroup(acrResourceGroupName)
  params: {
	acrName: acrName
	pullPrincipalId: pullIdentity.properties.principalId
	publishPrincipalId: publishIdentity.properties.principalId
	infraPrincipalId: infraIdentity.properties.principalId
  }
}

resource pullIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: '${appName}-pull'
  location: location
}

resource publishIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: '${appName}-publish'
  location: location
}

resource infraIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: '${appName}-infra'
  location: location
}

var federation = {
  issuer: 'https://token.actions.githubusercontent.com'
  subject: 'repo:${githubRepository}:environment:${githubEnvironment}'
  audiences: [
	'api://AzureADTokenExchange'
  ]
}

resource publishFederation 'Microsoft.ManagedIdentity/userAssignedIdentities/federatedIdentityCredentials@2024-11-30' = {
  parent: publishIdentity
  name: 'github-environment'
  properties: federation
}

resource infraFederation 'Microsoft.ManagedIdentity/userAssignedIdentities/federatedIdentityCredentials@2024-11-30' = {
  parent: infraIdentity
  name: 'github-environment'
  properties: federation
}

var contributorRoleId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')

resource infraRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, infraIdentity.id, contributorRoleId)
  properties: {
	roleDefinitionId: contributorRoleId
	principalId: infraIdentity.properties.principalId
	principalType: 'ServicePrincipal'
  }
}

output resourceGroupName string = resourceGroup().name
output registryName string = acrName
output containerAppName string = appName
output pullIdentityName string = pullIdentity.name
output publishClientId string = publishIdentity.properties.clientId
output infraClientId string = infraIdentity.properties.clientId
output tenantId string = tenant().tenantId
output subscriptionId string = subscription().subscriptionId

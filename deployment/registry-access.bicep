targetScope = 'resourceGroup'

@description('Name of the existing registry. This module does not change its configuration.')
param acrName string

param pullPrincipalId string
param publishPrincipalId string
param infraPrincipalId string

resource registry 'Microsoft.ContainerRegistry/registries@2025-04-01' existing = {
  name: acrName
}

var acrPullRoleId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')
var acrPushRoleId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions', '8311e382-0749-4cb8-b61a-304f252e45ec')
var readerRoleId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions', 'acdd72a7-3385-48ef-bd42-f606fba81ae7')

resource pullRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(registry.id, pullPrincipalId, acrPullRoleId)
  scope: registry
  properties: {
	roleDefinitionId: acrPullRoleId
	principalId: pullPrincipalId
	principalType: 'ServicePrincipal'
  }
}

resource publishRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(registry.id, publishPrincipalId, acrPushRoleId)
  scope: registry
  properties: {
	roleDefinitionId: acrPushRoleId
	principalId: publishPrincipalId
	principalType: 'ServicePrincipal'
  }
}

resource publishReaderRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(registry.id, publishPrincipalId, readerRoleId)
  scope: registry
  properties: {
	roleDefinitionId: readerRoleId
	principalId: publishPrincipalId
	principalType: 'ServicePrincipal'
  }
}

resource infraReaderRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(registry.id, infraPrincipalId, readerRoleId)
  scope: registry
  properties: {
	roleDefinitionId: readerRoleId
	principalId: infraPrincipalId
	principalType: 'ServicePrincipal'
  }
}

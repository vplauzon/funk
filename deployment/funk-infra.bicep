targetScope = 'resourceGroup'

@description('Region for the Container App and its managed environment.')
param location string = resourceGroup().location

@description('Name of the existing Azure Container Registry.')
param acrName string

@description('Resource group containing the existing registry, in this subscription.')
param acrResourceGroupName string

@description('Name of the Container App.')
@minLength(2)
@maxLength(32)
param appName string = 'funk-workbench'

@description('Independent name of the managed environment, shareable by multiple apps.')
@minLength(1)
param environmentName string

@description('Name of the existing image-pull identity in this resource group.')
param pullIdentityName string = '${appName}-pull'

@description('Full image reference, including its immutable sha256 digest.')
@minLength(1)
param image string

resource registry 'Microsoft.ContainerRegistry/registries@2025-04-01' existing = {
  name: acrName
  scope: resourceGroup(acrResourceGroupName)
}

resource pullIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' existing = {
  name: pullIdentityName
}

resource environment 'Microsoft.App/managedEnvironments@2025-07-01' = {
  name: environmentName
  location: location
  properties: {
	appLogsConfiguration: {
	  destination: ''
	}
	workloadProfiles: [
	  {
		name: 'Consumption'
		workloadProfileType: 'Consumption'
	  }
	]
  }
}

resource app 'Microsoft.App/containerApps@2025-07-01' = {
  name: appName
  location: location
  identity: {
	type: 'UserAssigned'
	userAssignedIdentities: {
	  '${pullIdentity.id}': {}
	}
  }
  properties: {
	environmentId: environment.id
	workloadProfileName: 'Consumption'
	configuration: {
	  activeRevisionsMode: 'Single'
	  ingress: {
		external: true
		targetPort: 8080
		transport: 'auto'
		allowInsecure: false
	  }
	  registries: [
		{
		  server: registry.properties.loginServer
		  identity: pullIdentity.id
		}
	  ]
	}
	template: {
	  containers: [
		{
		  name: 'workbench'
		  image: image
		  resources: {
			cpu: json('0.25')
			memory: '0.5Gi'
		  }
		  env: [
			{
			  name: 'ASPNETCORE_ENVIRONMENT'
			  value: 'Production'
			}
			{
			  name: 'ASPNETCORE_HTTP_PORTS'
			  value: '8080'
			}
		  ]
		  probes: [
			{
			  type: 'Startup'
			  httpGet: {
				path: '/'
				port: 8080
			  }
			  periodSeconds: 2
			  timeoutSeconds: 2
			  failureThreshold: 60
			}
			{
			  type: 'Readiness'
			  httpGet: {
				path: '/'
				port: 8080
			  }
			  periodSeconds: 5
			  timeoutSeconds: 2
			  failureThreshold: 3
			}
		  ]
		}
	  ]
	  scale: {
		minReplicas: 0
		maxReplicas: 1
		cooldownPeriod: 300
		rules: [
		  {
			name: 'http'
			http: {
			  metadata: {
				concurrentRequests: '10'
			  }
			}
		  }
		]
	  }
	}
  }
}

output appUrl string = 'https://${app.properties.configuration.ingress.fqdn}'
output appResourceId string = app.id

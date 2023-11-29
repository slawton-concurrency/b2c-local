param location string
param resourceGroupName string
param functionAppName string
param runtime string
param tableStorageUrl string
param storageConnectionString string

module functionAppDeployment 'modules/azureFunction/azureFunction.bicep' = {
  name: 'functionAppDeployment'
  params: {
    location: location
    resourceGroupName: resourceGroupName
    functionAppName: functionAppName
    runtime: runtime
    tableStorageUrl: tableStorageUrl
    storageConnectionString: storageConnectionString
  }
}

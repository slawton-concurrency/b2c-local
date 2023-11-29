param location string
param resourceGroupName string
param functionAppName string
param runtime string
param storageConnectionString string
param tableStorageUrl string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${functionAppName}-asp'
  location: location
  properties: {
    targetWorkerCount: 1
    targetWorkerSizeId: 1
    reserved: true
  }
  sku: {
    capacity: 1
    name: 'S2'
  }
  kind: 'linux'
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  location: location
  kind: 'FunctionApp'
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageConnectionString
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: runtime
        }
        {
          name: 'TableStorageURL'
          value: tableStorageUrl
        }
      ]
    }
  }
}

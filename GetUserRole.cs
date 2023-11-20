using Azure.Data.Tables;
using Azure;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
public class UserRolesRequest
{
    public string? version { get; set; }
    public int status { get; set; }
    public string? email { get; set; }
}

public class UserRolesResponse
{

    public string? version { get; set; }
    public int status { get; set; }
    public string? email { get; set; }
    public string? role { get; set; }
}

public class B2CResponseError
{
    public string version { get; set; } = "1.0.0";
    public int status { get; set; } = 409;
    public string? message { get; set; }
}

public class UserRoleEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string Role { get; set; }
    public ETag ETag { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}

public static class GetUserRole
{
    private static readonly string TableUrl = Environment.GetEnvironmentVariable("TableURL");
    private static readonly string ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    [Function("GetUserRole")]
    public static async Task<UserRolesResponse> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var emailRequest = JsonConvert.DeserializeObject<UserRolesRequest>(requestBody);

        if (string.IsNullOrWhiteSpace(emailRequest?.email))
        {
            return new UserRolesResponse
            {
                version = "1.0.0",
                status = 400,
                email = emailRequest?.email,
                role = null
            };
        }

        var tableClient = new TableServiceClient(ConnectionString).GetTableClient("tableb2clocal");
        await tableClient.CreateIfNotExistsAsync();

        string partitionKey = "User";
        string rowKey = emailRequest.email;

        try
        {
            var tableRes = await tableClient.GetEntityAsync<UserRoleEntity>(partitionKey, rowKey);

            return new UserRolesResponse
            {
                version = "1.0.0",
                status = 200,
                email = emailRequest.email,
                role = tableRes.Value.Role
            };

        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            var newUserRole = new UserRoleEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Role = "user"
            };

            await tableClient.AddEntityAsync(newUserRole);

            return new UserRolesResponse
            {
                version = "1.0.0",
                status = 200,
                email = emailRequest.email,
                role = newUserRole.Role
            };
        }
    }
}

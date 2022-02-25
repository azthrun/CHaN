using Microsoft.Azure.Cosmos;

namespace Inventory.Api.Data;

public class CosmosContainerProvider
{
    private readonly string databaseId;
    private readonly CosmosClient client;

    public CosmosContainerProvider(string connectionString, string databaseId, CosmosClientOptions? options = null)
    {
        client = new(connectionString, options);
        this.databaseId = databaseId;
    }

    public async Task<Container> GetContainerAsync(string containerId)
    {
        var database = client.GetDatabase(databaseId);
        await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
        return database.GetContainer(containerId);
    }
}

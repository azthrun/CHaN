namespace Inventory.Api.Data;

public static class CosmosServiceCollectionExtentions
{
    public static IServiceCollection UseCosmos(this IServiceCollection collection, string connectionString, string databaseId, CosmosClientOptions? options = null)
    {
        CosmosContainerProvider containerProvider = new(connectionString, databaseId, options);
        collection.AddSingleton<CosmosContainerProvider>(containerProvider);
        return collection;
    }
}
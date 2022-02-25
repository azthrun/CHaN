using Inventory.Api.Data;
using Inventory.Api.Exceptions;
using User = Inventory.Api.Models.User;
using Microsoft.Azure.Cosmos;

namespace Inventory.Api.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(CosmosContainerProvider containerProvider) : base(containerProvider)
    {
    }

    public async Task<User?> GetUserAsync(string email)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        try
        {
            QueryDefinition query = new QueryDefinition("SELECT * FROM Item c WHERE c.email = @email")
                .WithParameter("@email", email);
            List<User> results = new();
            using FeedIterator<User> resultSet = container.GetItemQueryIterator<User>(query
                , requestOptions: new QueryRequestOptions() { PartitionKey = new(User.PK.ToLower()), MaxItemCount = 1 });
            while (resultSet.HasMoreResults)
            {
                FeedResponse<User> response = await resultSet.ReadNextAsync();
                results.AddRange(response);
            }
            return results.FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }
}
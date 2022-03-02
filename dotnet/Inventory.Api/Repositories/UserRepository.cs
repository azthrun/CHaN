namespace Inventory.Api.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(CosmosContainerProvider containerProvider) : base(containerProvider)
    {
    }

    public override Task DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<User>> QueryAsync(long timestamp, bool includeDeleted)
    {
        throw new NotImplementedException();
    }

    public override async Task<User> ReadAsync(string email)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        List<User> results = new();
        try
        {
            QueryDefinition query = new QueryDefinition("SELECT * FROM Item c WHERE c.email = @email")
                .WithParameter("@email", email);
            using FeedIterator<User> resultSet = container.GetItemQueryIterator<User>(query
                , requestOptions: new QueryRequestOptions() { PartitionKey = new(User.PK.ToLower()), MaxItemCount = 1 });
            while (resultSet.HasMoreResults)
            {
                FeedResponse<User> response = await resultSet.ReadNextAsync();
                results.AddRange(response);
            }
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
        if (!results.Any()) throw new NotFoundException();
        return results.First();
    }
}
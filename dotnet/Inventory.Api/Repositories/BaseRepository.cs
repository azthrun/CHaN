using Inventory.Api.Abstractions;
using Inventory.Api.Data;
using Inventory.Api.Exceptions;
using Inventory.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Inventory.Api.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
{
    private readonly CosmosContainerProvider? containerProvider;
    protected Container? container;

    public BaseRepository(CosmosContainerProvider? containerProvider)
    {
        this.containerProvider = containerProvider;
    }

    protected async Task InitAsync()
    {
        if (container is null)
        {
            if (containerProvider is null) throw new RepositoryException("Container provider is not set");
            container = await containerProvider.GetContainerAsync(typeof(TEntity).Name);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> QueryAsync(long timestamp, bool includeDeleted)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        try
        {
            List<TEntity> results = new();
            var query = $"SELECT * FROM c WHERE c._ts >= {timestamp} {(!includeDeleted ? " AND c.deleted = false" : null)}";
            var iterator = container.GetItemQueryIterator<TEntity>(query);
            while (iterator.HasMoreResults)
            {
                var entity = await iterator.ReadNextAsync();
                results.AddRange(entity.Resource);
            }
            return results;
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        if (entity is null) throw new ArgumentNullException(typeof(TEntity).Name);
        try
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }
            else
            {
                var storeEntity = await container.ReadItemAsync<TEntity>(entity.Id, new(entity.PartitionKey)).ConfigureAwait(false);
                if (storeEntity is not null) throw new ConflictException(storeEntity);
            }
            entity.UpdatedAt = DateTimeOffset.Now;
            await container.CreateItemAsync(entity, new(entity.PartitionKey)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        if (entity is null) throw new BadRequestException();
        try
        {
            await container.DeleteItemAsync<TEntity>(entity.Id, new(entity.PartitionKey)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }

    public virtual async Task<TEntity> ReadAsync(string id)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        if (string.IsNullOrEmpty(id)) throw new BadRequestException();
        try
        {
            var test = typeof(TEntity).Name.ToLower();
            var entity = await container.ReadItemAsync<TEntity>(id, new(typeof(TEntity).Name.ToLower())).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException();
            return entity;
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }

    public virtual async Task UpdateAsync(string id, TEntity entity)
    {
        await InitAsync();
        if (container is null) throw new RepositoryException("Container is not set");
        if (entity is null) throw new ArgumentNullException(typeof(TEntity).Name);
        try
        {
            entity.UpdatedAt = DateTimeOffset.Now;
            await container.ReplaceItemAsync(entity, id, new(entity.PartitionKey)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }
}
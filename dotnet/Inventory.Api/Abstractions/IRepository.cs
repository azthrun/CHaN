namespace Inventory.Api.Abstractions;

public interface IRepository<TEntity> where TEntity : IModel
{
    Task CreateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<IEnumerable<TEntity>> QueryAsync(long timestamp, bool includeDeleted);
    Task<TEntity> ReadAsync(string id);
    Task UpdateAsync(string id, TEntity entity);
}

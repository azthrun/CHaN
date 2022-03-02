using Microsoft.AspNetCore.Authorization;

namespace Inventory.Api.Controllers;

[Authorize]
[ApiController]
public class DataController<TEntity> : ControllerBase where TEntity : IModel
{
    public DataController(IRepository<TEntity>? repository = null)
    {
        this.repository = repository;
    }

    public ILogger? Logger { get; set; }

    private IRepository<TEntity>? repository;
    public IRepository<TEntity> Repository
    {
        get => repository ?? throw new InvalidOperationException("Repository Is Not Set");
        set => repository = value ?? throw new ArgumentNullException(nameof(Repository));
    }

    [HttpPost]
    [ActionName("CreateAsync")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TEntity? entity)
    {
        if (entity is null) return BadRequest(new InvalidDataException("entity cannot be Null"));
        Logger?.LogInformation($"{nameof(CreateAsync)} - initiated");
        try
        {
            await Repository.CreateAsync(entity).ConfigureAwait(false);
            Logger?.LogInformation($"{nameof(CreateAsync)} - item created with Id: {entity.Id}");
            return CreatedAtAction(nameof(CreateAsync), new { id = entity.Id }, entity);
        }
        catch (ConflictException ex)
        {
            return StatusCode(ex.StatusCode, ex.Payload);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("{id}/{softDelete}")]
    [ActionName("DeleteAsync")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public virtual async Task<IActionResult> DeleteAsync([FromRoute] string id, [FromRoute] bool softDelete = true)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest(new InvalidDataException("id cannot be Empty"));
        Logger?.LogInformation($"{nameof(DeleteAsync)} - initiated");
        try
        {
            var entityInStore = await Repository.ReadAsync(id).ConfigureAwait(false); ;
            // if (entityInStore is null)
            // {
            //     Logger?.LogWarning($"{nameof(DeleteAsync)} ({id}) - entity not found");
            //     return NotFound();
            // }
            if (string.IsNullOrEmpty(entityInStore.Id)) throw new InvalidDataException("Invalid entity id");
            if (softDelete)
            {
                Logger?.LogInformation($"{nameof(DeleteAsync)} ({id}) - marking entity as deleted");
                entityInStore.Deleted = true;
                await Repository.UpdateAsync(entityInStore.Id, entityInStore).ConfigureAwait(false);
            }
            else
            {
                Logger?.LogInformation($"{nameof(DeleteAsync)} ({id}) - deleting entity");
                await Repository.DeleteAsync(entityInStore).ConfigureAwait(false);
            }
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            Logger?.LogWarning($"{nameof(DeleteAsync)} ({id}) - entity not found");
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{ts}/{includeDeleted}")]
    [ActionName("QueryAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> QueryAsync([FromRoute] long? ts, [FromRoute] bool includeDeleted = false)
    {
        if (!ts.HasValue) return BadRequest(new InvalidDataException("updatedAt cannot be Null"));
        try
        {
            Logger?.LogInformation($"{nameof(QueryAsync)} - initiated");
            var dataSet = await Repository.QueryAsync(ts.Value, includeDeleted);
            var count = dataSet.Count();
            Logger?.LogInformation($"{nameof(QueryAsync)} - returning {count} result{(count >= 2 ? "s" : null)}");
            return Ok(await Task.FromResult(dataSet.AsEnumerable()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{id}")]
    [ActionName("ReadAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> ReadAsync([FromRoute] string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest(new InvalidDataException("id cannot be Empty"));
        Logger?.LogInformation($"{nameof(ReadAsync)} - initiated");
        try
        {
            var entityInStore = await Repository.ReadAsync(id).ConfigureAwait(false); ;
            if (entityInStore.Deleted is not null && entityInStore.Deleted.Value)
            {
                Logger?.LogWarning($"{nameof(ReadAsync)} ({id}) - entity was marked as deleted");
                return StatusCode(StatusCodes.Status410Gone);
            }
            Logger?.LogInformation($"{nameof(ReadAsync)} - returning entity");
            return Ok(entityInStore);
        }
        catch (NotFoundException ex)
        {
            Logger?.LogWarning($"{nameof(ReadAsync)} ({id}) - entity not found");
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("{id}")]
    [ActionName("UpdateAsync")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] TEntity entity)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest(new InvalidDataException("id cannot be Empty"));
        if (entity is null) return BadRequest(new InvalidDataException("entity cannot be Null"));
        Logger?.LogInformation($"{nameof(UpdateAsync)} - initiated");
        try
        {
            var entityInStore = await Repository.ReadAsync(id).ConfigureAwait(false); ;
            if (entityInStore.Deleted is not null && entityInStore.Deleted.Value)
            {
                Logger?.LogWarning($"{nameof(UpdateAsync)} ({id}) - entity was marked as deleted");
                return StatusCode(StatusCodes.Status410Gone);
            }
            await Repository.UpdateAsync(id, entity).ConfigureAwait(false);
            Logger?.LogInformation($"{nameof(UpdateAsync)} - entity updated");
            return Ok(entity);
        }
        catch (NotFoundException ex)
        {
            Logger?.LogWarning($"{nameof(UpdateAsync)} ({id}) - entity not found");
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
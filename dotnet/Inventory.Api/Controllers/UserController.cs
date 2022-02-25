using Inventory.Api.Data;
using Inventory.Api.Exceptions;
using Inventory.Api.Models;
using Inventory.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("[controller]")]
public class UserController : DataController<User>
{
    public UserController(ILogger<UserController> logger, CosmosContainerProvider containerProvider)
        : base(new UserRepository(containerProvider))
    {
        Logger = logger;
    }

    public override async Task<IActionResult> CreateAsync([FromBody] User? user)
    {
        if (user is null) return BadRequest(new InvalidDataException("entity cannot be Null"));
        Logger?.LogInformation($"{nameof(CreateAsync)} - initiated");
        try
        {
            if (string.IsNullOrEmpty(user.Email)) return BadRequest(new InvalidDataException("email cannot be Null"));
            var entityInStore = await ((UserRepository)Repository).GetUserAsync(user.Email).ConfigureAwait(false);
            if (entityInStore is not null)
            {
                if (entityInStore.Deleted.HasValue && entityInStore.Deleted.Value)
                {
                    if (string.IsNullOrEmpty(entityInStore.Id)) throw new InvalidDataException("Invalid entity id");
                    entityInStore.Deleted = false;
                    await Repository.UpdateAsync(entityInStore.Id, entityInStore);
                }
                else return StatusCode(StatusCodes.Status403Forbidden, $"{nameof(CreateAsync)} - email already exists");
            }
            await Repository.CreateAsync(user).ConfigureAwait(false);
            Logger?.LogInformation($"{nameof(CreateAsync)} - item created with Id: {user.Id}");
            return CreatedAtAction(nameof(CreateAsync), new { id = user.Id }, user);
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

    public override async Task<IActionResult> DeleteAsync([FromRoute] string id, [FromRoute] bool softDelete = true)
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, await Task.FromResult($"{nameof(DeleteAsync)} - Not allow to delete"));
    }

    public override async Task<IActionResult> QueryAsync([FromRoute] long? ts, [FromRoute] bool includeDeleted = false)
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, await Task.FromResult($"{nameof(QueryAsync)} - Not allow to query"));
    }

    [HttpGet("{email}")]
    [ActionName("ReadAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<IActionResult> ReadAsync([FromRoute] string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest(new InvalidDataException("email cannot be Empty"));
        Logger?.LogInformation($"{nameof(ReadAsync)} - initiated");
        try
        {
            var entityInStore = await ((UserRepository)Repository).GetUserAsync(email).ConfigureAwait(false);
            if (entityInStore is null)
            {
                return NotFound();
            }
            if (entityInStore.Deleted is not null && entityInStore.Deleted.Value)
            {
                Logger?.LogWarning($"{nameof(ReadAsync)} ({email}) - entity was marked as deleted");
                return StatusCode(StatusCodes.Status410Gone);
            }
            Logger?.LogInformation($"{nameof(ReadAsync)} - returning entity");
            return Ok(entityInStore);
        }
        catch (NotFoundException ex)
        {
            Logger?.LogWarning($"{nameof(ReadAsync)} ({email}) - entity not found");
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}

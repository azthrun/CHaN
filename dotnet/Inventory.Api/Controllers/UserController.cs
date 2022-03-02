using Inventory.Api.Abstractions;
using Inventory.Api.Exceptions;
using Inventory.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("[controller]")]
public class UserController : DataController<User>
{
    public UserController(ILogger<UserController> logger, IRepository<User> repository) : base(repository)
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
            await Repository.ReadAsync(user.Email).ConfigureAwait(false);
            return StatusCode(StatusCodes.Status403Forbidden, $"{nameof(CreateAsync)} - email already exists");
        }
        catch (NotFoundException)
        {
            try
            {
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
    }

    public override async Task<IActionResult> DeleteAsync([FromRoute] string id, [FromRoute] bool softDelete = true)
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, await Task.FromResult($"{nameof(DeleteAsync)} - Not allow to delete"));
    }

    public override async Task<IActionResult> QueryAsync([FromRoute] long? ts, [FromRoute] bool includeDeleted = false)
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, await Task.FromResult($"{nameof(QueryAsync)} - Not allow to query"));
    }
}

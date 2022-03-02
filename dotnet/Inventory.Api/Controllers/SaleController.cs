namespace Inventory.Api.Controllers;

[Route("[controller]")]
public class SaleController : DataController<Sale>
{
    public SaleController(ILogger<SaleController> logger, IRepository<Sale> repository) : base(repository)
    {
        Logger = logger;
    }

    public override async Task<IActionResult> DeleteAsync([FromRoute] string id, [FromRoute] bool softDelete = true)
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, await Task.FromResult($"{nameof(DeleteAsync)} - Not allow to delete"));
    }
}

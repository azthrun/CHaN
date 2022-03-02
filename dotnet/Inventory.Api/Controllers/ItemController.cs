namespace Inventory.Api.Controllers;

[Route("[controller]")]
public class ItemController : DataController<Item>
{
    public ItemController(ILogger<ItemController> logger, IRepository<Item> repository) : base(repository)
    {
        Logger = logger;
    }
}

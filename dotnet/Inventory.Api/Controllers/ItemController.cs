using Inventory.Api.Data;
using Inventory.Api.Models;
using Inventory.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("[controller]")]
public class ItemController : DataController<Item>
{
    public ItemController(ILogger<ItemController> logger, CosmosContainerProvider containerProvider)
        : base(new ItemRepository(containerProvider))
    {
        Logger = logger;
    }
}

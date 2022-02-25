using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Repositories;

public class ItemRepository : BaseRepository<Item>
{
    public ItemRepository(CosmosContainerProvider containerProvider) : base(containerProvider)
    {
    }
}

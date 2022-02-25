using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Repositories;

public class SaleRepository : BaseRepository<Sale>
{
    public SaleRepository(CosmosContainerProvider containerProvider) : base(containerProvider)
    {
    }
}

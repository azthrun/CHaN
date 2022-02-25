namespace Inventory.Api.Models;

public class Item : BaseModel
{
    public const string PK = nameof(Item);

    public Item()
    {
        PartitionKey = PK.ToLower();
    }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? StockLocation { get; set; }
    public int? Quantity { get; set; }
    public double? BuyInPrice { get; set; }
    public int? HistorySold { get; set; }
    public string? ImagePath { get; set; }
}

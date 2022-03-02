namespace Inventory.Api.Models;

public class Sale : BaseModel
{
    public const string PK = nameof(Sale);

    private Sale()
    {
        PartitionKey = PK.ToLower();
    }

    public Sale(string itemId) : this()
    {
        ItemId = itemId;
        Quantity = 1;
        Date = DateTime.Now;
    }

    public string? ItemId { get; set; }
    public int? Quantity { get; set; }
    public double? Price { get; set; }
    public DateTime? Date { get; set; }
    public string? Buyer { get; set; }
    public string? Address { get; set; }
    public string? Tracking { get; set; }
    public string? Remarks { get; set; }
}
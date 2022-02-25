namespace Inventory.Api.Models;

public class User : BaseModel
{
    public const string PK = nameof(User);

    private User()
    {
        PartitionKey = PK.ToLower();
    }

    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public KeyValuePair<bool, DateTime> ActiveSession { get; set; }
}
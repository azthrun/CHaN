using Inventory.Api.Abstractions;
using Newtonsoft.Json;

namespace Inventory.Api.Models;

public class BaseModel : IModel
{
    public BaseModel()
    {
        Id = Guid.NewGuid().ToString();
        Deleted = false;
    }

    public string? Id { get; set; }
    public string? PartitionKey { get; set; }
    [JsonProperty("_ts")]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.UnixDateTimeConverter))]
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool? Deleted { get; set; }
}

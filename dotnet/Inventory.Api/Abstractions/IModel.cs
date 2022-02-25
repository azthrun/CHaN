namespace Inventory.Api.Abstractions;

public interface IModel
{
    string? Id { get; set; }
    string? PartitionKey { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
    bool? Deleted { get; set; }
}
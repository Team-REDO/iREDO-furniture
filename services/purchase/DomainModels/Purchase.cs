using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Purchase.Models;

public class Purchase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string PaymentsProvider { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "DKK";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAt { get; set; }

    public Order Order { get; set; } = new();
}
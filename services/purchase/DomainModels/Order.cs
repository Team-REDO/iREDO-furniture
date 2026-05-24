using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Purchase.Enums;

namespace Purchase.Models;

public class Order
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string UserGuid { get; set; } = string.Empty;

    public OrderStatus OrderStatus { get; set; }

    public List<OrderItem> OrderItems { get; set; } = [];

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
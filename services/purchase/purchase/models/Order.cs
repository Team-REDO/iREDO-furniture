﻿using DTO;
using Furnitures;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Purchase.Enums;

public class Order
{
    [BsonId]
    public string? orderId { get; set; } = Guid.NewGuid().ToString();

    public string UserGuid { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public OrderStatus OrderStatus { get; set; }

    public List<FurnitureMessage> OrderItems { get; set; } = new();

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
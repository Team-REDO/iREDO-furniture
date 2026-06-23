namespace DTO;

public class CreateCheckoutRequest
{
    public string Email { get; set; } = string.Empty;
    public string OrderGuid { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "pending";
    public List<CreateCheckoutItem> OrderItems { get; set; } = new();
}

public class CreateCheckoutItem
{
    public string Title { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
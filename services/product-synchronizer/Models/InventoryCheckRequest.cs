public class InventoryCheckRequest
{
    public string EventType { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string OrderId { get; set; } = string.Empty;
}
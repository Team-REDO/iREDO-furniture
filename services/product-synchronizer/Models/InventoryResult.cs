public class InventoryResult
{
    public string EventType { get; set; } = "InventoryResult";
    public string ProductId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string Reason { get; set; } = string.Empty;
}
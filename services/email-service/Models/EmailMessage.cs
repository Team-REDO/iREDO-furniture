namespace EmailService.Models;

public class EmailMessage
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;
    public string PurchaseStatus { get; set; } = string.Empty;

    public List<EmailItem> Items { get; set; } = new();
}

public class EmailItem
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
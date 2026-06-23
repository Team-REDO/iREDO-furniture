namespace DTO
{
    public class EmailMessage
    {
    public string To { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string PurchaseStatus { get; set; }

    public List<EmailItem> EmailItems { get; set; }
    }
}
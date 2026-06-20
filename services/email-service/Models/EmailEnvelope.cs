using EmailService.Models;

public class EmailEnvelope
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public string EventType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public EmailMessage Payload { get; set; } = new();
}
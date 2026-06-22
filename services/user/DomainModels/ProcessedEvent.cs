namespace UserService.DomainModels
{
    public class ProcessedEvent
    {
        public Guid EventId { get; set; }

        public string EventType { get; set; } = "";

        public DateTime ProcessedAt { get; set; }
    }
}
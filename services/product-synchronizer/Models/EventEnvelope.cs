public class EventEnvelope<T>
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public string EventType { get; set; } = string.Empty;
    public T Payload { get; set; } = default!;
}
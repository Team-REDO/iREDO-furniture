namespace user.Messaging.Events;

public class UserUpdatedEvent
{
    public Guid EventId { get; set; }
    public Guid PersonGuid { get; set; }
}
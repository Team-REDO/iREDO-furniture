namespace user.Messaging.Events;

public class UserRemovedEvent
{
    public Guid EventId { get; set; }
    public Guid PersonGuid { get; set; }
}
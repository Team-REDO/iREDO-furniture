namespace user.Messaging.Events;

public class SalesPostRemovedEvent
{
    public Guid EventId { get; set; }
    public Guid SalesPostGuid { get; set; }
}
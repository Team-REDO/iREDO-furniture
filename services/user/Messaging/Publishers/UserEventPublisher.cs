using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using user.Messaging.Events;
using user.Services;

namespace user.Messaging.Publishers;

public class UserEventPublisher
{
    private readonly RabbitMqService _rabbitMq;

    public UserEventPublisher(RabbitMqService rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    public async Task PublishUserRemoved(UserRemovedEvent evt)
    {
        var channel = await _rabbitMq.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "redo.events",
            type: ExchangeType.Topic,
            durable: true);

        var json = JsonSerializer.Serialize(evt);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "redo.events",
            routingKey: "user.removed",
            body: body);
    }

    public async Task PublishUserUpdated(UserUpdatedEvent evt)
    {
        var channel = await _rabbitMq.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "redo.events",
            type: ExchangeType.Topic,
            durable: true);

        var json = JsonSerializer.Serialize(evt);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "redo.events",
            routingKey: "user.updated",
            body: body);
    }
}
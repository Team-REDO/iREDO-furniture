using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using user.Data;
using user.Messaging.Events;
using user.Services;
using UserService.DomainModels;
namespace user.Messaging.Consumers
{
    public class SalesPostRemovedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqService _rabbitMq;

        public SalesPostRemovedConsumer(
            IServiceScopeFactory scopeFactory,
            RabbitMqService rabbitMq)
        {
            _scopeFactory = scopeFactory;
            _rabbitMq = rabbitMq;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            IChannel channel = null;

            while (channel == null && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    channel = await _rabbitMq.CreateChannelAsync();
                }
                catch
                {
                    Console.WriteLine(
                        "RabbitMQ not ready. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            await channel.ExchangeDeclareAsync(
                exchange: "redo.events",
                type: ExchangeType.Topic,
                durable: true);

            await channel.QueueDeclareAsync(
                queue: "salesPost.removed",
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: "salesPost.removed",
                exchange: "redo.events",
                routingKey: "salesPost.removed");

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var evt = JsonSerializer.Deserialize<SalesPostRemovedEvent>(json);

                if (evt == null)
                    return;
                if (evt.EventId == Guid.Empty)
                {
                    Console.WriteLine("EventId missing");
                    return;
                }

                using var scope = _scopeFactory.CreateScope();

                var db = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var alreadyProcessed =
                    await db.Processed_Events
                        .AnyAsync(x => x.EventId == evt.EventId);
                
                if (alreadyProcessed)
                {
                    Console.WriteLine(
                        $"Skipping duplicate event {evt.EventId}");

                    return;
                }

                Console.WriteLine($"\nReceived salesPost.removed: {evt.SalesPostGuid}\n"); //Add temporary logging

                var savedPosts = db.Saved_List_Posts
                    .Where(x => x.SalesPostGuid == evt.SalesPostGuid);

                db.Saved_List_Posts.RemoveRange(savedPosts);
                db.Processed_Events.Add(
                    new ProcessedEvent
                    {
                        EventId = evt.EventId,
                        EventType = "salesPost.removed"
                    }
                );

                await db.SaveChangesAsync();
            };

            await channel.BasicConsumeAsync(
                queue: "salesPost.removed",
                autoAck: true,
                consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}

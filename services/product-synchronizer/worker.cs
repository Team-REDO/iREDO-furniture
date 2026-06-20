using System.Linq;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMongoService _mongo;
    private IConnection _connection;
    private IModel _channel;

    public Worker(ILogger<Worker> logger, IMongoService mongo, bool skipRabbit)
    {
        _logger = logger;
        _mongo = mongo;

        if (skipRabbit)
        {
            return; // ✅ unchanged
        }

        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
        var queueName = Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "listing_queue";

        var factory = new ConnectionFactory()
        {
            HostName = host
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public Worker(ILogger<Worker> logger, IMongoService mongo)
    {
        _logger = logger;
        _mongo = mongo;

        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
        var queueName = Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "listing_queue";

        var factory = new ConnectionFactory()
        {
            HostName = host
        };

        int retries = 5;

        while (retries > 0)
        {
            try
            {
                _logger.LogInformation("Connecting to RabbitMQ...");

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _logger.LogInformation("Connected!");

                _channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                return;
            }
            catch
            {
                retries--;
                _logger.LogWarning("Retrying in 5 seconds...");
                Thread.Sleep(5000);
            }
        }

        throw new Exception("Could not connect to RabbitMQ");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueName = Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "listing_queue";

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                _logger.LogInformation("Message received");

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                // ✅ FIX 1: Correct envelope deserialization
                var envelope = JsonSerializer.Deserialize<EventEnvelope<IncomingListing>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (envelope == null)
                {
                    _logger.LogWarning("Invalid envelope");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                var input = envelope.Payload;

                // ✅ FIX 2: Idempotency check (NEW)
                using (var db = new SynchronizerDbContext())
                {
                    var exists = db.ProcessedEvents
                        .Any(e => e.EventId == envelope.EventId);

                    if (exists)
                    {
                        _logger.LogInformation("Event already processed — skipping");
                        _channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }
                }

                // ❌ REMOVED BROKEN LINE:
                // var input = envelope.Payload;, new JsonSerializerOptions...

                if (input == null)
                {
                    _logger.LogWarning("Invalid input");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                // ✅ FIX 3: use envelope.EventType instead
                if (string.IsNullOrEmpty(envelope.EventType))
                {
                    _logger.LogWarning("Missing eventType");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                // ✅ FIX 4: use envelope.EventType in switch
                switch (envelope.EventType)
                {
                    case "ListingCreated":
                    case "ListingUpdated":
                        await HandleUpsert(input);
                        break;

                    case "ListingDeleted":
                        await _mongo.DeleteAsync(input.Guid);
                        break;

                    default:
                        _logger.LogWarning("Unknown event type: {EventType}", envelope.EventType);
                        break;
                }

                // ✅ FIX 5: Save processed event (NEW)
                using (var db = new SynchronizerDbContext())
                {
                    db.ProcessedEvents.Add(new ProcessedEvent
                    {
                        EventId = envelope.EventId,
                        EventType = envelope.EventType,
                        ProcessedAt = DateTime.UtcNow
                    });

                    db.SaveChanges();
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Processing failed");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );

        return Task.CompletedTask;
    }

    public async Task HandleUpsert(IncomingListing input)
    {
        var details = input.ListingDetails ?? new ListingDetails();

        var color = details.Colors?.FirstOrDefault();
        var sub = details.SubCategories?.FirstOrDefault();

        var post = new SalesPost
        {
            Guid = input.Guid,
            PersonId = input.PersonGUID,
            Title = details.Title,
            Description = details.Description,
            Quantity = details.Quantity.ToString(),
            Price = details.Price,
            Condition = details.Condition,
            ZipCode = details.City,

            Color = color != null
                ? new ColorDb
                {
                    Name = color.Name,
                    Href = color.Href
                }
                : new ColorDb(),

            Category = sub != null
                ? new CategoryDb
                {
                    Name = sub.Category.Name,
                    Subcat = new SubCategoryDb
                    {
                        Name = sub.Name
                    }
                }
                : new CategoryDb(),

            Images = details.Images?
                .Select(url => new ImageDb { Url = url })
                .ToList() ?? new List<ImageDb>()
        };

        await _mongo.UpsertAsync(post);

        _logger.LogInformation("Upserted {Guid}", post.Guid);
    }
}
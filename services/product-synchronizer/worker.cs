using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SynchronizerService.Models;
using System.Drawing;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using SynchronizerService.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMongoService _mongo;
    private readonly IServiceProvider _serviceProvider;

    private IConnection _connection;
    private IModel _channel;

    public Worker(
        ILogger<Worker> logger,
        IMongoService mongo,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _mongo = mongo;
        _serviceProvider = serviceProvider;

        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
        var queueName = Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "listing_queue";

        var factory = new ConnectionFactory() { HostName = host };

        while (true)
        {
            try
            {
                _logger.LogInformation("Connecting to RabbitMQ...");

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                _logger.LogInformation("Connected!");
                break;
            }
            catch
            {
                _logger.LogWarning("RabbitMQ not ready... retrying in 5 seconds");
                Thread.Sleep(5000);
            }
        }
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

                _logger.LogInformation("Raw message: {Json}", json);

                var envelope = JsonSerializer.Deserialize<EventEnvelope<IncomingListing>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (envelope == null)
                {
                    _logger.LogWarning("Invalid envelope");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                // DB via DI (IMPORTANT FIX)
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SynchronizerDbContext>();

                var exists = db.ProcessedEvents
                    .Any(e => e.EventId == envelope.EventId);

                if (exists)
                {
                    _logger.LogInformation("Event already processed — skipping");
                    _channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                if (string.IsNullOrEmpty(envelope.EventType))
                {
                    _logger.LogWarning("Missing eventType");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                var input = envelope.Payload;

                if (input == null)
                {
                    _logger.LogWarning("Invalid payload");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                // EVENT HANDLING
                switch (envelope.EventType)
                {
                    case "ListingCreated":
                    case "ListingUpdated":
                        await HandleUpsert(input);
                        break;

                    case "ListingDeleted":
                        await _mongo.DeleteAsync(input.Guid);
                        break;

                    case "ProductCreated":
                        _logger.LogInformation("Handling ProductCreated");

                        // logging
                        _logger.LogInformation("Product payload: {Payload}", json);

                        break;

                    default:
                        _logger.LogWarning("Unknown event type: {EventType}", envelope.EventType);
                        break;
                }

                // SAVE EVENT (same DI db)
                db.ProcessedEvents.Add(new ProcessedEvent
                {
                    EventId = envelope.EventId,
                    EventType = envelope.EventType,
                    ProcessedAt = DateTime.UtcNow
                });

                await db.SaveChangesAsync();

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

        var post = new SalesPost
        {
            SalesPostGuid = input.Guid,
            PersonGuid = input.PersonGUID,
            Title = details.Title,
            Description = details.Description,
            Size = details.Size,
            Quantity = details.Quantity,
            Price = details.Price,
            Condition = details.Condition,
            City = details.City,
            ModifiedAt = DateTime.UtcNow,

            //  COLORS (LIST)
            Colors = details.Colors?
                .Select(c => new ColorDb
                {
                    ColorGuid = Guid.NewGuid().ToString(), // temp GUID
                    Name = c.Name,
                    Href = c.Href
                })
                .ToList() ?? new List<ColorDb>(),

            // CATEGORIES (LIST WITH SUBCATEGORIES)
            Categories = details.SubCategories?
                .Select(sub => new CategoryDb
                {
                    CategoryGuid = Guid.NewGuid().ToString(), // temp GUID
                    CategoryName = sub.Category.Name,

                    Subcategories = new List<SubCategoryDb>
                    {
                    new SubCategoryDb
                    {
                        SubcategoryGuid = Guid.NewGuid().ToString(),
                        SubcategoryName = sub.Name
                    }
                    }
                })
                .ToList() ?? new List<CategoryDb>(),

            // IMAGES (LIST)
            Images = details.Images?
                .Select(url => new ImageDb
                {
                    ImageGuid = Guid.NewGuid().ToString(),
                    ImageUrl = url
                })
                .ToList() ?? new List<ImageDb>()
        };

        await _mongo.UpsertAsync(post);

        _logger.LogInformation("Upserted {Guid}", post.SalesPostGuid);
    }
}
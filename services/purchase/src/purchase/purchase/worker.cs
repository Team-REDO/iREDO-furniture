using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Purchase.Models;
using Purchase.Enums;
using service;
using models;
using service.Grapql;
using DTO;
using service.interfaces;

public class PurchaseConsumerWorker : BackgroundService
{
    private readonly ILogger<PurchaseConsumerWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly OrderService orderService;
    private readonly IRabbitPublisher rabbitPublisher;

    private IConnection? _connection;
    private IModel? _channel;

    public PurchaseConsumerWorker(
        ILogger<PurchaseConsumerWorker> logger,
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory,IRabbitPublisher publisher)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
        rabbitPublisher=publisher;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "purchase_queue",
            durable: true,
            exclusive: false,
            autoDelete: false);
        _channel.QueueDeclare(queue: "purchase.failed",
            durable: true,
            exclusive: false,
            autoDelete: false);

        _logger.LogInformation("PurchaseConsumerWorker started");

        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
{
    try
    {
        var body = ea.Body.ToArray();
        var json = Encoding.UTF8.GetString(body);

        var evt = JsonSerializer.Deserialize<EventEnvelope<Order>>(json);
        
        if (evt?.payload == null)
        {
            _logger.LogWarning("Invalid message received");
            return;
        }
        

        using var scope = _scopeFactory.CreateScope();

        HandleMessage(evt, scope);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error processing message");
    }
};

        _channel.BasicConsume(
            queue: "purchase_queue",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }

private void HandleMessage(EventEnvelope<Order> evt, IServiceScope scope)
{
    _logger.LogInformation("Order received: {@Order}", evt.payload.OrderItems);
    var processedService = scope.ServiceProvider.GetRequiredService<IProcessedEventService>();


    try
    {
        switch (evt.eventType)
        {
            case "ItemReserved":
                HandleOrderCreated(evt);
                break;

            case "ItemReservedFailed":
                HandleOrderCancelled(evt, scope);
                break;

            default:
                _logger.LogWarning("Unknown event type: {Type}", evt.eventType);
                return;
        }

        // ONLY mark processed if everything succeeded
        processedService.MarkProcessed(evt.eventId);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed processing event {Id}", evt.eventId);
        throw;
    }
}


private async Task HandleOrderCancelled(EventEnvelope<Order> evt, IServiceScope scope)
{
    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

    evt.payload.OrderStatus = OrderStatus.Cancelled;

    await orderService.UpdateOrder(evt.payload);
    await rabbitPublisher.PublishAsync(evt.payload,"purchase.failed");
    await rabbitPublisher.PublishAsync(evt.payload,"purchase_failed");


    _logger.LogInformation("Order cancelled: {Id}", evt.payload.orderId);

    
}

private async Task HandleOrderCreated(EventEnvelope<Order> evt)
{

    _logger.LogInformation("Order received: {@Order}", evt.payload.OrderItems.First().Title);

    var client = _httpClientFactory.CreateClient();
    var json = JsonSerializer.Serialize(evt.payload, new JsonSerializerOptions
{
    PropertyNamingPolicy = null, // keep exact casing (VERY IMPORTANT)
    PropertyNameCaseInsensitive = true
});

var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(
    "http://localhost:8080/purchase",
    content
);
    if (!response.IsSuccessStatusCode)
    {
        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Purchase API failed: {Error}", error);
        throw new Exception("Purchase API call failed");
    }

    var result = await response.Content.ReadFromJsonAsync<Checkout>();

    if (result == null || string.IsNullOrEmpty(result.CheckoutUrl))
    {
        _logger.LogError("Invalid checkout response");
        throw new Exception("Invalid checkout response");
    }

    _logger.LogInformation("Checkout created: {Url}", result.CheckoutUrl);
    evt.payload.CheckoutUrl=result.CheckoutUrl;
    await orderService.UpdateOrder(evt.payload);
    
}
}
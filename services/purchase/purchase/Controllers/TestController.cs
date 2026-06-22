using DotNetEnv;
using DTO;
using Microsoft.AspNetCore.Mvc;
using models;
using mutation;
using Purchase.Enums;
using Purchase.Models;
using RabbitMQ.Client;
using service;
using service.Grapql;
using service.interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IRabbitPublisher _publisher;
    private readonly IEventEnvelopeService<OrderCreated> _envelopeService;
    private readonly IOrderService _orderService;

    public TestController(
        IRabbitPublisher publisher,
        IEventEnvelopeService<OrderCreated> envelopeService,
        IOrderService orderService,
        ILogger<TestController> logger)
    {
        _publisher = publisher;
        _envelopeService = envelopeService;
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> TestPurchase([FromBody] OrderCreated request)
    {
        // FIX 1: proper validation
        if (request == null)
            return BadRequest("Request is null");

        if (string.IsNullOrWhiteSpace(request.UserId))
            return BadRequest("UserId is required");

        if (string.IsNullOrWhiteSpace(request.OrderId))
            return BadRequest("OrderId is required");

        // FIX 2: build order properly
        var order = new Order
        {
            orderId = request.OrderId,
            UserGuid = request.UserId,
            OrderStatus = OrderStatus.Pending,
            Email = request.Email
        };

        string guid= Guid.NewGuid().ToString();
        var correlationId = Guid.NewGuid().ToString();

        // FIX 4: event envelope correct naming
        var envelope = new EventEnvelope<OrderCreated>
        {
            eventId = guid,
            eventType = "OrderCreated",
            eventVersion = 1,
            occurredAt = DateTime.UtcNow,
            producer = "purchase",
            correlationId = correlationId,
            causationId = guid,
            payload = request,
            published = false
        };
        
        if(_orderService.GetOrderById(request.OrderId)!=null)
        {
            Ok();
        }

        await _orderService.AddOrder(order);
        await _envelopeService.AddEvent(envelope);

        await _publisher.PublishAsync(envelope, "OrderCreated");

        _logger.LogInformation("OrderCreated test event sent for {OrderId}", request.OrderId);

        return Ok("Test purchase created");
    }
}


using DTO;
using Microsoft.AspNetCore.Mvc;
using service.interfaces;
using System.Collections.Concurrent;

[ApiController]
[Route("purchase")]
public class PurchaseController : ControllerBase
{
    private readonly StripeService _stripe;
    private readonly IRabbitPublisher _publisher;

    private static readonly ConcurrentDictionary<string, string> ProcessedOrders = new();

    public PurchaseController(StripeService stripe, IRabbitPublisher publisher)
    {
        _stripe = stripe;
        _publisher = publisher;
    }

    [HttpPost("checkout")]
    public IActionResult CreateCheckout([FromBody] CreateCheckoutRequest order)
    {
        if (string.IsNullOrWhiteSpace(order.OrderGuid))
            return BadRequest(new { message = "orderGuid is required." });

        if (order.OrderItems == null || !order.OrderItems.Any())
            return BadRequest(new { message = "Order must contain at least one item." });

        if (ProcessedOrders.TryGetValue(order.OrderGuid, out var existingCheckoutUrl))
        {
            return Ok(new
            {
                checkoutUrl = existingCheckoutUrl,
                duplicate = true,
                message = "Duplicate orderGuid. Returning existing checkout URL."
            });
        }

        var checkoutUrl = _stripe.CreateCheckoutSession(order);

        ProcessedOrders.TryAdd(order.OrderGuid, checkoutUrl);

        return Ok(new
        {
            checkoutUrl,
            duplicate = false,
            message = "New checkout session created."
        });
    }

    [HttpPost("payment-success")]
public async Task<IActionResult> PaymentSuccess([FromBody] PaymentSuccessRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Email))
        return BadRequest(new { message = "Email is required." });

    if (string.IsNullOrWhiteSpace(request.OrderGuid))
        return BadRequest(new { message = "OrderGuid is required." });

    var emailEvent = new
    {
        eventId = Guid.NewGuid().ToString(),
        eventType = "EmailRequested",
        eventVersion = 1,
        occurredAt = DateTime.UtcNow,
        producer = "PurchaseService",
        correlationId = request.OrderGuid,
        causationId = request.OrderGuid,
        payload = new
        {
            to = request.Email,
            subject = "Your iREDO purchase was completed",
            body = $"Thank you for your purchase of {request.Title}. Your order {request.OrderGuid} was completed successfully."
        }
    };

    await _publisher.PublishAsync(emailEvent, "email_queue");

    return Ok(new
    {
        message = "Payment success handled. Email event published.",
        queue = "email_queue",
        orderGuid = request.OrderGuid
    });
}

    [HttpPost("demo-email")]
    public async Task<IActionResult> PublishDemoEmail([FromBody] CreateCheckoutRequest order)
    {
        var itemTitle = order.OrderItems.FirstOrDefault()?.Title ?? "your furniture item";

        var emailEvent = new
        {
            eventId = Guid.NewGuid().ToString(),
            eventType = "EmailRequested",
            eventVersion = 1,
            occurredAt = DateTime.UtcNow,
            producer = "PurchaseService",
            correlationId = order.OrderGuid,
            causationId = order.OrderGuid,
            payload = new
            {
                to = order.Email,
                subject = "Your iREDO purchase was completed",
                body = $"Thank you for your purchase of {itemTitle}. Your order {order.OrderGuid} was completed successfully."
            }
        };

        await _publisher.PublishAsync(emailEvent, "email_queue");

        return Ok(new
        {
            message = "EmailRequested event published to RabbitMQ",
            queue = "email_queue",
            orderGuid = order.OrderGuid
        });
    }
}
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

[ApiController]
[Route("purchase")]
public class PurchaseController : ControllerBase
{
    private readonly StripeService _stripe = new();

    private static readonly ConcurrentDictionary<string, string> ProcessedOrders = new();

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
}
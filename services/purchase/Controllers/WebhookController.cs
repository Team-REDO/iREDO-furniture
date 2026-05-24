using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("webhook")]
public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        var secret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

        if (string.IsNullOrEmpty(secret))
        {
            Console.WriteLine("Webhook secret is missing");
            return BadRequest("Webhook secret not configured");
        }

        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        if (string.IsNullOrEmpty(signature))
        {
            Console.WriteLine("Missing Stripe-Signature header");
            return BadRequest("Missing Stripe-Signature header");
        }

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signature,
                secret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;

                if (session == null)
                {
                    Console.WriteLine("Session is null");
                    return BadRequest();
                }

                var productId = session.Metadata.ContainsKey("productId")
                    ? session.Metadata["productId"]
                    : "unknown";

                var quantity = session.Metadata.ContainsKey("quantity")
                    ? int.Parse(session.Metadata["quantity"])
                    : 0;

                Console.WriteLine($"Payment success for {productId}");

                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq"
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                var queueName = Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "listing_queue";

                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var message = new
                {
                    eventType = "PurchaseCompleted",
                    guid = productId,
                    quantity = quantity
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine("Message sent to RabbitMQ");
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            Console.WriteLine("Stripe error: " + ex.Message);
            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine("General error: " + ex.Message);
            return BadRequest();
        }
    }
}
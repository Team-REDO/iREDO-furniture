using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text;
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

        try
        {
            var signature = Request.Headers["Stripe-Signature"];

            if (string.IsNullOrEmpty(signature))
            {
                return BadRequest("Missing Stripe-Signature header");
            }

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signature,
                secret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                var productId = session.Metadata["productId"];
                var quantity = int.Parse(session.Metadata["quantity"]);

                Console.WriteLine($"Payment success for {productId}");

                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq"
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "listing_queue",
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

                channel.BasicPublish("", "listing_queue", null, body);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }
    }
}
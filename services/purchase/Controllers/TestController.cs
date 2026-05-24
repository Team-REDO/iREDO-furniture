using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpPost("purchase")]
    public IActionResult TestPurchase([FromBody] TestPurchaseRequest request)
    {
        var factory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost"
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
            guid = request.ProductId,
            quantity = request.Quantity
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish("", queueName, null, body);

        Console.WriteLine("TEST message sent to RabbitMQ");

        return Ok("Test message sent");
    }
}

public class TestPurchaseRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
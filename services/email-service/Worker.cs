using EmailService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using EmailService.Data;
using EmailService.Service;
using Microsoft.EntityFrameworkCore;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IModel _channel;
    private readonly IEmailSender _sender;
    private readonly IAiEmailGenerator _ai;
    private readonly EmailDbContext _db;

    public Worker(
        ILogger<Worker> logger,
        IEmailSender sender,
        IAiEmailGenerator ai,
        EmailDbContext db)
    {
        _sender = sender;
        _ai = ai;
        _logger = logger;
        _db = db;

        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq"
        };

        while (true)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                break;
            }
            catch
            {
                Console.WriteLine("RabbitMQ not ready yet... retrying in 5 seconds");
                Thread.Sleep(5000);
            }
        }

        _channel.QueueDeclare(
            queue: "email_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            Console.WriteLine("Message received!");

            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Raw message: {json}");

            var envelope = JsonSerializer.Deserialize<EmailEnvelope>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (envelope == null)
            {
                Console.WriteLine("Invalid envelope");
                return;
            }

            // CHECK IF ALREADY PROCESSED
            var exists = _db.ProcessedEvents
                .Any(e => e.EventId == envelope.EventId);

            if (exists)
            {
                Console.WriteLine("Event already processed — skipping");
                return;
            }

            var email = envelope.Payload;

            Console.WriteLine($"Processing email to {email.To}");

            // Build a detailed context for the AI using the full payload            
            var aiContext = BuildAiContext(email);

            string finalBody = email.Body;

            try
            {
                Console.WriteLine("Generating AI email...");

                // aiContext
                finalBody = await _ai.GenerateEmail(email.Subject, aiContext);

                Console.WriteLine("AI generation succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("AI failed:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Using fallback body");

                // If AI fails, use the structured order info instead of only "Hi"
                finalBody = aiContext;
            }

            _sender.Send(email.To, email.Subject, finalBody);

            // SAVE PROCESSED EVENT
            _db.ProcessedEvents.Add(new ProcessedEvent
            {
                EventId = envelope.EventId,
                EventType = envelope.EventType,
                ProcessedAt = DateTime.UtcNow
            });

            _db.SaveChanges();
        };

        _channel.BasicConsume(
            queue: "email_queue",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    // helper method.
    // This converts the deserialized EmailMessage object into useful text for the AI.
    private string BuildAiContext(EmailMessage email)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"Recipient email: {email.To}");
        builder.AppendLine($"Customer email: {email.CustomerEmail}");
        builder.AppendLine($"Purchase status: {email.PurchaseStatus}");
        builder.AppendLine($"Original body/message: {email.Body}");
        builder.AppendLine();

        if (email.Items != null && email.Items.Any())
        {
            builder.AppendLine("Purchased items:");

            decimal total = 0;

            foreach (var item in email.Items)
            {
                var lineTotal = item.Quantity * item.UnitPrice;
                total += lineTotal;

                builder.AppendLine(
                    $"- {item.Name}: quantity {item.Quantity}, unit price {item.UnitPrice}, line total {lineTotal}"
                );
            }

            builder.AppendLine();
            builder.AppendLine($"Total price: {total}");
        }
        else
        {
            builder.AppendLine("No item details were provided.");
        }

        return builder.ToString();
    }
}

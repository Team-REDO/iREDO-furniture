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

            string finalBody = email.Body;

            try
            {
                Console.WriteLine("Generating AI email...");
                finalBody = await _ai.GenerateEmail(email.Subject, email.Body);
                Console.WriteLine("AI generation succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("AI failed:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Using fallback body");
            }

            _sender.Send(email.To, email.Subject, finalBody);

            //SAVE PROCESSED EVENT
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
}
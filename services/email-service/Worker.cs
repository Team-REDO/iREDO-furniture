using EmailService.Data;
using EmailService.Models;
using EmailService.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEmailSender _sender;
    private readonly IAiEmailGenerator _ai;
    private readonly IServiceProvider _serviceProvider;

    private IConnection _connection;
    private IModel _channel;

    public Worker(
        ILogger<Worker> logger,
        IEmailSender sender,
        IAiEmailGenerator ai,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _sender = sender;
        _ai = ai;
        _serviceProvider = serviceProvider;

        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";

        var factory = new ConnectionFactory()
        {

            HostName = host
        };

        while (true)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                Console.WriteLine("✅ Connected to RabbitMQ!");

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

            var envelope = JsonSerializer.Deserialize<EmailEnvelope>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (envelope == null)
            {
                Console.WriteLine("Invalid envelope");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();

                var exists = db.ProcessedEvents
                    .Any(e => e.EventId == envelope.EventId);

                if (exists)
                {
                    Console.WriteLine("Event already processed — skipping");
                    return;
                }
            }

            var email = envelope.Payload;

            string finalBody = email.Body.ToString();

            try
            {
                finalBody = await _ai.GenerateEmail(email.Subject,email.Body.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("AI FAILED:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            _sender.Send(email.To, email.Subject, finalBody);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();

                db.ProcessedEvents.Add(new ProcessedEvent
                {
                    EventId = envelope.EventId,
                    EventType = envelope.EventType,
                    ProcessedAt = DateTime.UtcNow
                });

                db.SaveChanges();
            }
        };

        _channel.BasicConsume(
            queue: "email_queue",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}
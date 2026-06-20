using EmailService.Models;
using EmailService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IModel _channel;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

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

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

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

            var email = JsonSerializer.Deserialize<EmailMessage>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (email == null)
            {
                Console.WriteLine("Failed to deserialize email");
                return;
            }

            Console.WriteLine($"Processing email to {email.To}");

            string finalBody = email.Body; 

            try
            {
                Console.WriteLine("Generating AI email...");

                var ai = new AiEmailGenerator();

                finalBody = await ai.GenerateEmail(email.Subject, email.Body);

                Console.WriteLine("AI generation succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("AI failed:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Using fallback body");
            }

            var sender = new EmailSender();
            sender.Send(email.To, email.Subject, finalBody);
        };

        _channel.BasicConsume(
            queue: "email_queue",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}
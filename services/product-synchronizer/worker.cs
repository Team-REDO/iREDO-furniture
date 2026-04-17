using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class Worker : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;

    public Worker()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq"
        };

        int retries = 5;

        while (retries > 0)
        {
            try
            {
                Console.WriteLine("Connecting to RabbitMQ...");
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                Console.WriteLine("Connected!");

                // Declare queues AFTER successful connection
                _channel.QueueDeclare(
                    queue: "inventory_check_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                _channel.QueueDeclare(
                    queue: "inventory_result_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                return; 
            }
            catch
            {
                retries--;
                Console.WriteLine("Failed to connect. Retrying in 5 seconds...");
                Thread.Sleep(5000);
            }
        }

        throw new Exception("Could not connect to RabbitMQ");
    


    _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "inventory_check_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        _channel.QueueDeclare(
            queue: "inventory_result_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            Console.WriteLine("Message received");

            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            Console.WriteLine("Raw message: " + json);

            var request = JsonSerializer.Deserialize<InventoryCheckRequest>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request == null)
            {
                Console.WriteLine("Invalid request");
                return;
            }

            var (available, reason) = FakeInventory.Check(request.ProductId, request.Quantity);

            var result = new InventoryResult
            {
                ProductId = request.ProductId,
                OrderId = request.OrderId,
                IsAvailable = available,
                Reason = reason
            };

            var responseJson = JsonSerializer.Serialize(result);

            Console.WriteLine("Publishing result: " + responseJson);

            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            _channel.BasicPublish(
                exchange: "",
                routingKey: "inventory_result_queue",
                basicProperties: null,
                body: responseBytes
            );
        };

        _channel.BasicConsume(
            queue: "inventory_check_queue",
            autoAck: true,
            consumer: consumer
        );

        return Task.CompletedTask;
    }
}
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using service.interfaces;

public class RabbitPublisher : IDisposable, IRabbitPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly object _lock = new();

    public RabbitPublisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            AutomaticRecoveryEnabled = true,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.BasicQos(0, 100, false);
    }

    public Task PublishAsync<T>(T message, string queueName)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var props = _channel.CreateBasicProperties();
        props.Persistent = true;

        lock (_lock)
        {
            // Important for demo: make sure queue exists even if email service is stopped.
            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: props,
                body: body);
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
using RabbitMQ.Client;

namespace user.Services
{

    public class RabbitMqService
    {
        private readonly IConfiguration _configuration;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            var host = _configuration["RABBITMQ_HOST"]
                ?? throw new Exception("RABBITMQ_HOST missing");

            var user = _configuration["RABBITMQ_USER"]
                ?? throw new Exception("RABBITMQ_USER missing");

            var password = _configuration["RABBITMQ_PASSWORD"]
                ?? throw new Exception("RABBITMQ_PASSWORD missing");

            var factory = new ConnectionFactory
            {
                HostName = host,
                UserName = user,
                Password = password
            };

            var connection = await factory.CreateConnectionAsync();
            return await connection.CreateChannelAsync();
        }
    }
}
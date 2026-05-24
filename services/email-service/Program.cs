using EmailService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();


// To run RabbitMQ locally, use the following command:
// Make sure to have Docker installed and running on your machine, then execute the command in your terminal.
//navigate to the directory where you store the RabbitMQ Docker image, then execute the command in your terminal to start a RabbitMQ
// container with the management plugin enabled. This will allow you to access the RabbitMQ management interface at
// http://localhost:15672/ using the default credentials (username: guest, password: guest).

// docker run -d --hostname rabbit --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

/*
This is the current expected format.
{
"To": "yourreal@email.com",
  "Subject": "RabbitMQ Email Test",
  "Body": "If you received this email, the C# RabbitMQ microservice works."
}
*/

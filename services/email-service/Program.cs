using EmailService.Service;
using EmailService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Use environment variable instead of appsettings
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("MYSQL_CONNECTION is not set");

// register DbContext
builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseMySql(
    connectionString,
    new MySqlServerVersion(new Version(8, 0, 0)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure()
));

// your services
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAiEmailGenerator, AiEmailGenerator>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
    db.Database.EnsureCreated(); // creates tables
}

host.Run();

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
  "eventId": "123",
  "eventType": "EmailRequested",
  "createdAt": "2026-06-18T12:00:00Z",
  "payload": {
    "to": "test@test.com",
    "subject": "Hello",
    "body": "Hi"
  }
}

{
  "eventId": "test-1",
  "eventType": "EmailRequested",
  "payload": {
    "to": "mpfugl@hotmail.com",
    "subject": "Hello",
    "body": "Test message"
  }
}
*/

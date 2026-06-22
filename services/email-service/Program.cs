using EmailService.Data;
using EmailService.Service;
using EmailService.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

//  GET CONNECTION STRING
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("MYSQL_CONNECTION is not set");

//  REGISTER DB CONTEXT
builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

//  SERVICES
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAiEmailGenerator, AiEmailGenerator>();

//  WORKER
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

//  DB INIT
var maxRetries = 10;
var delay = TimeSpan.FromSeconds(5);

for (int i = 0; i < maxRetries; i++)
{
    try
    {
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();

            Console.WriteLine("Trying to connect to MySQL...");

            db.Database.EnsureCreated();
            Seeder.Seed(db);

            Console.WriteLine("Database ready!");
            break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("MySQL not ready... retrying in 5 seconds");
        Console.WriteLine(ex.Message);
        Thread.Sleep(delay);
    }
}

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
*/

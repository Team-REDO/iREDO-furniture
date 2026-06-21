using EmailService.Service;
using EmailService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// get connection string from env
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("MYSQL_CONNECTION is not set");

// FIX: no AutoDetect (prevents crash)
builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)))
);

// services
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAiEmailGenerator, AiEmailGenerator>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// ensure DB is created
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();

    var retries = 10;

    while (retries > 0)
    {
        try
        {
            Console.WriteLine("Trying to connect to MySQL...");
            db.Database.EnsureCreated();
            Console.WriteLine("Database ready!");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine("MySQL not ready yet... retrying in 5 seconds");
            Console.WriteLine(ex.Message);

            retries--;
            Thread.Sleep(5000);
        }
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
  "eventId": "order-1002", 
  "eventType": "EmailRequested", 
  "payload": { 
    "to": "mpfugl@hotmail.com", 
    "subject": "Order Confirmation", 
    "body": { 
      "customerName": "Michael", 
      "items": [ 
        { "name": "Oak Dining Table", "quantity": 1, "price": 4999 }, 
        { "name": "Dining Chair", "quantity": 4, "price": 799 } 
      ], 
      "total": 8195 
    } 
  } 
}
*/

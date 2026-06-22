using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SynchronizerService.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMongoService, MongoService>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<SynchronizerDbContext>();


var host = builder.Build();

var maxRetries = 10;
var delay = TimeSpan.FromSeconds(5);

for (int i = 0; i < maxRetries; i++)
{
    try
    {
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SynchronizerDbContext>();

            Console.WriteLine("Trying to connect to MySQL...");
            db.Database.EnsureCreated();

            Seeder.Seed(db);
        }

        Console.WriteLine("Database ready!");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine("MySQL not ready... retrying in 5 seconds");
        Console.WriteLine(ex.Message);
        Thread.Sleep(delay);
    }
}

host.Run();

/*
 current expected json update format:
 {
  "eventId": "sync-test-1",
  "eventType": "ProductCreated",
  "payload": {
    "name": "Test Chair",
    "price": 499,
    "quantity": 2
  }
}
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SynchronizerService.Services;

var builder = Host.CreateApplicationBuilder(args);

//GET CONNECTION STRING FROM ENV
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("MYSQL_CONNECTION is not set");

//REGISTER DB CONTEXT PROPERLY
builder.Services.AddDbContext<SynchronizerDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

//SERVICES
builder.Services.AddSingleton<IMongoService, MongoService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

//RETRY DB CONNECTION (KEEP THIS)
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
  "eventId": "test-listing-001",
  "eventType": "ListingCreated",
  "payload": {
    "guid": "listing-123",
    "personGUID": "user-456",
    "listingDetails": {
      "title": "Modern Chair",
      "description": "A very nice modern chair in great condition",
      "size": "Medium",
      "quantity": 2,
      "price": 499.99,
      "condition": "Used - Like New",
      "city": "Copenhagen",

      "colors": [
        {
          "name": "Black",
          "href": "/colors/black"
        },
        {
          "name": "White",
          "href": "/colors/white"
        }
      ],

      "subCategories": [
        {
          "name": "Chairs",
          "category": {
            "name": "Furniture"
          }
        }
      ],

      "images": [
        "https://example.com/image1.jpg",
        "https://example.com/image2.jpg"
      ]
    }
  }
}
 */
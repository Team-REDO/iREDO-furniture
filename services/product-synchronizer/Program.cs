using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMongoService, MongoService>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<SynchronizerDbContext>();

var host = builder.Build();

int retries = 5;

while (retries > 0)
{
    try
    {
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SynchronizerDbContext>();

            db.Database.EnsureCreated();
            Seeder.Seed(db);
        }

        Console.WriteLine("Database ready!");
        break;
    }
    catch (Exception ex)
    {
        retries--;

        Console.WriteLine("MySQL not ready... retrying in 5 seconds");
        Console.WriteLine(ex.Message);

        Thread.Sleep(5000);
    }
}

if (retries == 0)
{
    throw new Exception("Could not connect to MySQL after multiple retries");
}

host.Run();
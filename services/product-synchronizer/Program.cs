using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMongoService, MongoService>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<SynchronizerDbContext>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SynchronizerDbContext>();
    Seeder.Seed(db);
    db.Database.EnsureCreated();
}

host.Run();
using HotChocolate.Execution;
using MongoDB.Driver;
using productrepo;
using products;


var builder = WebApplication.CreateBuilder(args);

// Enable logging to console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var mongoConnection = builder.Configuration["MONGODB_CONNECTION"];
Console.WriteLine($"[STARTUP] MongoDB connection: {mongoConnection}");

builder.Services.AddSingleton<IMongoCollection<Furniture>>(sp =>
{
    var client = new MongoClient(mongoConnection);
    var database = client.GetDatabase("furnituredatabase");
    var collection = database.GetCollection<Furniture>("furniture");
    Console.WriteLine($"[STARTUP] Connected to collection: furniture");
    return collection;
});

builder.Services.AddSingleton<ProductRepo>();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<queries.Query>()
    .AddFiltering()
.AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGraphQL("/graphql");

using (var scope = app.Services.CreateScope())
{
    var executorResolver = scope.ServiceProvider
        .GetRequiredService<IRequestExecutorResolver>();
    var executor = await executorResolver.GetRequestExecutorAsync();
    var schema = executor.Schema.Print();
    File.WriteAllText("schema.graphql", schema);
}

app.Run();




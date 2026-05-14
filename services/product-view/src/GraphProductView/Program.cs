using MongoDB.Driver;
using productrepo;
using products;


var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = builder.Configuration["MONGODB_CONNECTION_STRING"] ?? "mongodb://mongodb:27017";

builder.Services.AddSingleton<IMongoCollection<Furniture>>(sp =>
{
    var client = new MongoClient(mongoConnectionString);
    var database = client.GetDatabase("furnituredatabase");
    return database.GetCollection<Furniture>("Furniture");
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

app.Run();





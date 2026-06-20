using HotChocolate.Execution;
using HotChocolate.Data.MongoDb;
using MongoDB.Driver;
using Furniturerepo;
using Furnitures;


var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
var mongoConnection = Environment.GetEnvironmentVariable("database");

var test = new SalesPost();

builder.Services.AddSingleton<IMongoCollection<SalesPost>>(sp =>
{
    var client = new MongoClient(mongoConnection);
    var database = client.GetDatabase("furnituredatabase");
    return database.GetCollection<SalesPost>("furniture");
});

builder.Services.AddSingleton<FurnitureRepo>();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<queries.Query>()
    .AddPagingArguments()
    .AddFiltering()
    .AddSorting().AddMongoDbFiltering()
    .AddMongoDbSorting().ModifyRequestOptions(o =>
    {
        o.IncludeExceptionDetails = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGraphQL("/graphql");

app.Run();





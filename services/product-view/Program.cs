using HotChocolate.Execution;
using HotChocolate.Data.MongoDb;
using MongoDB.Driver;
using Furniturerepo;
using Furnitures;


var builder = WebApplication.CreateBuilder(args);
var mongoConnection = builder.Configuration["MONGO_CONNECTION"];

if (string.IsNullOrEmpty(mongoConnection))
{
    throw new Exception("Missing MONGO_CONNECTION environment variable");
}

builder.Services.AddSingleton<IMongoCollection<SalesPost>>(sp =>
{
    var client = new MongoClient(mongoConnection);

    var database = client.GetDatabase("furnitures");

    return database.GetCollection<SalesPost>("SalesPost");
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





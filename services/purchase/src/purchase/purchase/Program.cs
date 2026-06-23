using MongoDB.Driver;
using service;
using service.interfaces;
using service.Grapql;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
StripeConfiguration.ApiKey = builder.Configuration["STRIPE_SECRET_KEY"];

// MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("MongoDb");

    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();

    // Must match the database used in seed.js
    return client.GetDatabase("purchase");
});

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProcessedEventService, ProcessedEventService>();

builder.Services.AddScoped(
    typeof(IEventEnvelopeService<>),
    typeof(EventEnvelopeService<>));

builder.Services.AddScoped<StripeService>();

builder.Services.AddHttpClient();

// RabbitMQ
builder.Services.AddSingleton<IRabbitPublisher, RabbitPublisher>();

// Background worker
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddHostedService<PurchaseConsumerWorker>();
}

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<query.Query>()
    .AddMutationType<mutation.Mutation>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}



app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/api/graphql");

app.Run();
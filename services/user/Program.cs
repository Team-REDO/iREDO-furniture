using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading;
using user.Data;
using user.Messaging.Consumers;
using user.Messaging.Publishers;
using user.Services;


DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
// DB
/*var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? $"Server=localhost,{Environment.GetEnvironmentVariable("USER_DB_PORT")};Database={Environment.GetEnvironmentVariable("USER_DB_NAME")};User={Environment.GetEnvironmentVariable("USER_DB_USER")};Password={Environment.GetEnvironmentVariable("USER_DB_PASSWORD")};TrustServerCertificate=True";*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string not found");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    }));

// JWT
builder.Services.AddScoped<JwtService>();
var jwtKey = Environment.GetEnvironmentVariable("USER_API_Jwt_key")
    ?? throw new Exception("JWT key missing from environment variables");

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = "Cookies";
})
.AddCookie("Cookies") // 👈 REQUIRED FOR GOOGLE
.AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(jwtKey);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["token"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
})
.AddGoogle(options =>
{
    options.ClientId = Environment.GetEnvironmentVariable("USER_GOOGLE_CLIENT_ID");
    options.ClientSecret = Environment.GetEnvironmentVariable("USER_GOOGLE_CLIENT_SECRET");
    options.Scope.Add("email");
    options.Scope.Add("profile");
});
// RabbitMQ
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddScoped<UserEventPublisher>();
builder.Services.AddHostedService<SalesPostRemovedConsumer>();


// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "http://localhost:8080"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "user",
        Version = "1.0"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseAuthentication();//add 
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 10;

    while (retries > 0)
    {
        try
        {
            Console.WriteLine("Trying DB connection...");
            db.Database.Migrate();
            Console.WriteLine("DB ready!");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"DB not ready... retrying ({retries} left)");
            Thread.Sleep(5000);
        }
    }

    if (retries == 0)
    {
        throw new Exception("Database never became available. Congrats.");
    }
}

app.Run();

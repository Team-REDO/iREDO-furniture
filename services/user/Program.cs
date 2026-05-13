using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using user.Data;
using user.Services;
using System.Threading;
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
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

    // THIS is the important one
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
})
.AddGoogle(options =>
{
    options.ClientId = Environment.GetEnvironmentVariable("USER_GOOGLE_CLIENT_ID");
    options.ClientSecret = Environment.GetEnvironmentVariable("USER_GOOGLE_CLIENT_SECRET");
    options.Scope.Add("email");
    options.Scope.Add("profile");
});


// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
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

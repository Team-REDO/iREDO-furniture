using Amazon.S3;
using interfaces;
using middleware;
using service;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// optional but OK
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var accountId = builder.Configuration["R2:AccountId"];
var accessKey = builder.Configuration["R2:AccessKey"];
var secretKey = builder.Configuration["R2:SecretKey"];
var bucket = builder.Configuration["R2:Bucket"];

if (string.IsNullOrEmpty(accountId) ||
    string.IsNullOrEmpty(accessKey) ||
    string.IsNullOrEmpty(secretKey) ||
    string.IsNullOrEmpty(bucket))
{
    throw new Exception("R2 configuration is missing");
}

var s3 = R2ClientFactory.Create(accountId, accessKey, secretKey);

builder.Services.AddSingleton<IAmazonS3>(s3);
builder.Services.AddSingleton<IFileStorageClient>(
    new R2StorageClient(s3, bucket)
);

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.MapControllers();
app.Run();
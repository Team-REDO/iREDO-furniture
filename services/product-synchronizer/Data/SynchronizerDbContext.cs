using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

public class SynchronizerDbContext : DbContext
{
    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("MYSQL_CONNECTION is not set");

        options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(connectionString));
    }
}
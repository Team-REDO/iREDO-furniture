using Microsoft.EntityFrameworkCore;

public class EmailDbContext : DbContext
{
    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("MYSQL_CONNECTION is not set");

        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
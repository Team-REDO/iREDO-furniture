using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

public class SynchronizerDbContext : DbContext
{
    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var conn = Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

        if (string.IsNullOrEmpty(conn))
            throw new Exception("MYSQL_CONNECTION not set");

        options.UseMySql(
        conn,
        new MySqlServerVersion(new Version(8, 0, 0))
    );
    }
}
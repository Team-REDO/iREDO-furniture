using Microsoft.EntityFrameworkCore;

public class EmailDbContext : DbContext
{
    public EmailDbContext(DbContextOptions<EmailDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }
}
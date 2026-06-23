using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

public class SynchronizerDbContext : DbContext
{
    public SynchronizerDbContext(DbContextOptions<SynchronizerDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProcessedEvent> ProcessedEvents { get; set; }
}

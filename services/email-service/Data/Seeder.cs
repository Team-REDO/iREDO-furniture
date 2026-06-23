using EmailService.Models;

namespace EmailService.Data;

public static class Seeder
{
    public static void Seed(EmailDbContext db)
    {
        // If already seeded → do nothing
        if (db.ProcessedEvents.Any())
            return;

        var seedData = new List<ProcessedEvent>
        {
            new ProcessedEvent
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "EmailSent",
                ProcessedAt = DateTime.UtcNow.AddMinutes(-30)
            },
            new ProcessedEvent
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "EmailSent",
                ProcessedAt = DateTime.UtcNow.AddMinutes(-20)
            },
            new ProcessedEvent
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "EmailSent",
                ProcessedAt = DateTime.UtcNow.AddMinutes(-10)
            }
        };

        db.ProcessedEvents.AddRange(seedData);
        db.SaveChanges();

        Console.WriteLine("🌱 Database seeded with fake events");
    }
}
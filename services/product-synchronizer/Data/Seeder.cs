public static class Seeder
{
    public static void Seed(SynchronizerDbContext db)
    {
        if (!db.ProcessedEvents.Any())
        {
            db.ProcessedEvents.Add(new ProcessedEvent
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "ListingCreated",
                ProcessedAt = DateTime.UtcNow.AddDays(-1)
            });

            db.SaveChanges();
        }
    }
}
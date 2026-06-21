public static class Seeder
{
    public static void Seed(SynchronizerDbContext db)
    {
        int retries = 5;

        while (retries > 0)
        {
            try
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

                Console.WriteLine("Seeder completed successfully");
                return;
            }
            catch (Exception ex)
            {
                retries--;

                Console.WriteLine("MySQL not ready yet... retrying in 5 seconds");
                Console.WriteLine(ex.Message);

                Thread.Sleep(5000);
            }
        }

        throw new Exception("Could not connect to MySQL after multiple retries");
    }
}
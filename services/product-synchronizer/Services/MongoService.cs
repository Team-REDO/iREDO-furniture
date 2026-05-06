using MongoDB.Driver;

public class MongoService
{
    private readonly IMongoCollection<SalesPost> _collection;

public MongoService()
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION");
        Console.WriteLine("DEBUG ENV VALUE: " + (connectionString ?? "NULL"));

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("MONGO_CONNECTION is not set");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("marketplace");
        _collection = database.GetCollection<SalesPost>("sales_posts");
    }

    public async Task UpsertAsync(SalesPost post)
    {
        var filter = Builders<SalesPost>.Filter.Eq(x => x.Guid, post.Guid);

        await _collection.ReplaceOneAsync(
            filter,
            post,
            new ReplaceOptions { IsUpsert = true }
        );
    }

    public async Task DeleteAsync(string guid)
    {
        var filter = Builders<SalesPost>.Filter.Eq(x => x.Guid, guid);
        await _collection.DeleteOneAsync(filter);
    }

}

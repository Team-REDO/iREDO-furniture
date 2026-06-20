public interface IMongoService
{
    Task UpsertAsync(SalesPost post);
    Task DeleteAsync(string guid);
}
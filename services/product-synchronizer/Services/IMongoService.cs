using SynchronizerService.Models;

namespace SynchronizerService.Services
{
    public interface IMongoService
    {
        Task UpsertAsync(SalesPost post);
        Task DeleteAsync(string guid);
    }
}
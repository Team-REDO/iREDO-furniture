using MongoDB.Driver;
using Furnitures;

namespace Furniturerepo
{
    public class FurnitureRepo
    {
        private readonly IMongoCollection<SalesPost> _furnitureCollection;

        public FurnitureRepo(IMongoCollection<SalesPost> database)
        {
            _furnitureCollection = database;
        }

        
        public async Task< IQueryable<SalesPost>> GetAllFurnitureAsync()
        {
            try
            {
                return _furnitureCollection.AsQueryable();
            }
            catch (Exception ex)
            {
                // Log the error - schema mismatch or connection issues
                throw new Exception($"Database query failed: {ex.Message}", ex);
            }
        }
        
        public async Task<SalesPost> GetFurnitureByIdAsync(string id)
        {
            try
            {
                return await _furnitureCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Log the error - schema mismatch or connection issues
                throw new Exception($"Database query failed for ID {id}: {ex.Message}", ex);
            }
        }

    }
}
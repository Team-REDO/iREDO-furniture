using MongoDB.Driver;
using products;

namespace productrepo
{
    public class ProductRepo
    {
        private readonly IMongoCollection<Furniture> _furnitureCollection;

        public ProductRepo(IMongoCollection<Furniture> database)
        {
            _furnitureCollection = database;
        }

        
        public async Task<List<Furniture>> GetAllFurnitureAsync()
        {
            try
            {
                return await _furnitureCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error - schema mismatch or connection issues
                Console.WriteLine(ex.ToString());
                throw new Exception($"Database query failed: {ex.Message}", ex);
            }
        }
        
        public async Task<Furniture> GetFurnitureByIdAsync(string id)
        {
            try
            {
                return await _furnitureCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Log the error - schema mismatch or connection issues
                Console.WriteLine(ex.ToString());
                throw new Exception($"Database query failed for ID {id}: {ex.Message}", ex);
            }
        }

    }
}
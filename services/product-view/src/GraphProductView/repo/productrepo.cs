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
                Console.WriteLine("[QUERY] GetAllFurnitureAsync: Starting query...");
                Console.WriteLine($"[QUERY] Collection name: {_furnitureCollection.CollectionNamespace.CollectionName}");
                Console.WriteLine($"[QUERY] Database name: {_furnitureCollection.CollectionNamespace.DatabaseNamespace.DatabaseName}");
                
                var filter = Builders<Furniture>.Filter.Empty;
                Console.WriteLine($"[QUERY] Filter: {filter}");
                
                var result = await _furnitureCollection.Find(filter).ToListAsync();
                Console.WriteLine($"[QUERY] GetAllFurnitureAsync: Found {result.Count} items");
                
                if (result.Count > 0)
                {
                    var firstItem = result[0];
                    Console.WriteLine($"[QUERY] First item - Id: {firstItem.Id}, Title: {firstItem.title}");
                    Console.WriteLine($"[QUERY] First item all fields: {System.Text.Json.JsonSerializer.Serialize(firstItem)}");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GetAllFurnitureAsync failed: {ex.Message}");
                Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[ERROR] InnerException: {ex.InnerException.Message}");
                    Console.WriteLine($"[ERROR] InnerException StackTrace: {ex.InnerException.StackTrace}");
                }
                throw;
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
                throw new Exception($"Database query failed for ID {id}: {ex.Message}", ex);
            }
        }

    }
}
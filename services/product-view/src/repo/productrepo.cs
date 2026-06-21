using MongoDB.Driver;
using Furnitures;

namespace Furniturerepo;

public class FurnitureRepo
{
    private readonly IMongoCollection<SalesPost> _salesPosts;
    private readonly ILogger<FurnitureRepo> _logger;

    public FurnitureRepo(IMongoCollection<SalesPost> salesPosts, ILogger<FurnitureRepo> logger)
    {
        _salesPosts = salesPosts;
        _logger = logger;
    }

    public IQueryable<SalesPost> GetAllFurniture()
    {
        try
        {
            return _salesPosts.AsQueryable();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create furniture query from MongoDB");
            throw new InvalidOperationException("Could not load furniture from Product View.", ex);
        }
    }

    public async Task<SalesPost?> GetFurnitureByIdAsync(string id)
    {
        try
        {
            return await _salesPosts
                .Find(f => f.Id == id || f.SalesPostGuid == id)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get furniture with id {Id}", id);
            throw new InvalidOperationException($"Could not load furniture with id {id}.", ex);
        }
    }
}

// namespace Furniturerepo
// {
//     public class FurnitureRepo
//     {
//         private readonly IMongoCollection<SalesPost> _furnitureCollection;

//         public FurnitureRepo(IMongoCollection<SalesPost> database)
//         {
//             _furnitureCollection = database;
//         }

        
//         public async Task< IQueryable<SalesPost>> GetAllFurnitureAsync()
//         {
//             try
//             {
//                 return _furnitureCollection.AsQueryable();
//             }
//             catch (Exception ex)
//             {
//                 // Log the error - schema mismatch or connection issues
//                 throw new Exception($"Database query failed: {ex.Message}", ex);
//             }
//         }
        
//         public async Task<SalesPost> GetFurnitureByIdAsync(string id)
//         {
//             try
//             {
//                 return await _furnitureCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
//             }
//             catch (Exception ex)
//             {
//                 // Log the error - schema mismatch or connection issues
//                 throw new Exception($"Database query failed for ID {id}: {ex.Message}", ex);
//             }
//         }

//     }
// }
using Furniturerepo;
using Furnitures;

namespace queries;

public class Query
{
    [UsePaging(DefaultPageSize = 10, MaxPageSize = 50, IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<SalesPost> GetAllFurniture([Service] FurnitureRepo repo)
    {
        return repo.GetAllFurniture();
    }

    public Task<SalesPost?> GetFurnitureById(string id, [Service] FurnitureRepo repo)
    {
        return repo.GetFurnitureByIdAsync(id);
    }
}


// using Furniturerepo;
// using Furnitures;

// namespace  queries
// {
//     public class Query
// {

//     // this method is used to get all the furniture items it's equipped with sorting, filtering and paging capabilities
//     //  making it able to sort and filter on all elements like category, price or color
//     [UsePaging]
//     [UseFiltering]
//     [UseSorting]
//     public Task< IQueryable<SalesPost>> GetAllFurniture(
//         [Service] FurnitureRepo repo)
//     {
//         var allFurniture = repo.GetAllFurnitureAsync();
//         return allFurniture;
//     }

//     // this method is used to get a single furniture item by its id, it returns the furniture item if found or null if not found
//     public Task<SalesPost> GetFurnitureById(
//         string
//          id,
//         [Service] FurnitureRepo repo)
//     {
//         return repo.GetFurnitureByIdAsync(id);
//     }
// }
// }
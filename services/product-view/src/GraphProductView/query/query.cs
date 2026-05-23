
using productrepo;
using products;

namespace  queries
{
    public class Query
{

    // this method is used to get all the furniture items it's equipped with sorting, filtering and paging capabilities
    //  making it able to sort and filter on all elements like category, price or color
    [UseSorting]
    [UseFiltering]
    [UsePaging]
    public async Task<List<Furniture>> GetAllFurniture(
        [Service] ProductRepo repo)
    {
        var allFurniture = await repo.GetAllFurnitureAsync();
        return allFurniture;
    }

    // this method is used to get a single furniture item by its id, it returns the furniture item if found or null if not found
    public async Task<Furniture> GetFurnitureById(
        string
         id,
        [Service] ProductRepo repo)
    {
        return await repo.GetFurnitureByIdAsync(id);
    }
}
}

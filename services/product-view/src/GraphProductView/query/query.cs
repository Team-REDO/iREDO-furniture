
using productrepo;
using products;

namespace  queries
{
    public class Query
{
    [UseSorting]
    [UseFiltering]
    public async Task<List<Furniture>> GetAllFurniture(
        [Service] ProductRepo repo)
    {
        var allFurniture = await repo.GetAllFurnitureAsync();
        return allFurniture;
    }

    public async Task<Furniture> GetFurnitureById(
        string
         id,
        [Service] ProductRepo repo)
    {
        return await repo.GetFurnitureByIdAsync(id);
    }
}
}

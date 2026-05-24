
public class OrderItem
{
    public string SalesPostGuid { get; set; } = default!;

    // Snapshot fields from Product service

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }

    public List<ImageModel> Images { get; set; } = [];

    public List<CategoryModel> Categories { get; set; } = [];
}
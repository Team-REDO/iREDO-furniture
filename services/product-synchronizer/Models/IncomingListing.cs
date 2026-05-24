public class IncomingListing
{
    public string EventType { get; set; } = string.Empty;
    public string Guid { get; set; } = string.Empty;
    public string PersonGUID { get; set; } = string.Empty;
    public ListingDetails ListingDetails { get; set; } = new();
}

public class ListingDetails
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public List<Color> Colors { get; set; } = new();
    public List<SubCategory> SubCategories { get; set; } = new();
    public List<string> Images { get; set; } = new();
}

public class Color
{
    public string Name { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
}

public class SubCategory
{
    public string Name { get; set; } = string.Empty;
    public Category Category { get; set; } = new();
}

public class Category
{
    public string Name { get; set; } = string.Empty;
}
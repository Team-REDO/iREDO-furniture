using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SalesPost
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string SalesPostGuid { get; set; } = System.Guid.NewGuid().ToString();
    public string Guid { get; set; } = string.Empty;
    public string PersonId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Size { get; set; } = "N/A";
    public int Quantity { get; set; }
    public double Price { get; set; }

    public string Condition { get; set; } = string.Empty;
    public string ZipCode { get; set; } = "0000";

    public Status Status { get; set; } = new();

    public ColorDb Color { get; set; } = new();
    public CategoryDb Category { get; set; } = new();

    public List<ImageDb> Images { get; set; } = new();
}

public class Status
{
    public bool Removed { get; set; } = false;
    public DateTime? Date { get; set; }
}

public class ColorDb
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
}

public class CategoryDb
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public SubCategoryDb Subcat { get; set; } = new();
}

public class SubCategoryDb
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}

public class ImageDb
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Url { get; set; } = string.Empty;
}
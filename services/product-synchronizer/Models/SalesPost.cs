using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

public class SalesPost
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("sales_post_guid")]
    public string SalesPostGuid { get; set; } = System.Guid.NewGuid().ToString();

    [BsonElement("guid")]
    public string Guid { get; set; } = string.Empty;

    [BsonElement("personId")]
    public string PersonId { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("size")]
    public string Size { get; set; } = "N/A";

    [BsonElement("quantity")]
    public string Quantity { get; set; } = "0";

    [BsonElement("price")]
    public double Price { get; set; }

    [BsonElement("condition")]
    public string Condition { get; set; } = string.Empty;

    [BsonElement("zip_code")]
    public string ZipCode { get; set; } = string.Empty;

    [BsonElement("status")]
    public Status Status { get; set; } = new();

    [BsonElement("color")]
    public ColorDb Color { get; set; } = new();

    [BsonElement("category")]
    public CategoryDb Category { get; set; } = new();

    [BsonElement("images")]
    public List<ImageDb> Images { get; set; } = new();
}

public class Status
{
    public bool Removed { get; set; } = false;
    public DateTime? Date { get; set; }
}

public class ColorDb
{
    public string Id { get; set; } = System.Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
}

public class CategoryDb
{
    public string Id { get; set; } = System.Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public SubCategoryDb Subcat { get; set; } = new();
}

public class SubCategoryDb
{
    public string Id { get; set; } = System.Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}

public class ImageDb
{
    public string Id { get; set; } = System.Guid.NewGuid().ToString();
    public string Url { get; set; } = string.Empty;
}
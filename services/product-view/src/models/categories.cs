using MongoDB.Bson.Serialization.Attributes;
using Subcategories;

namespace Categories;

[BsonIgnoreExtraElements]
public class Category
{
    [BsonElement("catid")]
    public string? CatId { get; set; }

    [BsonElement("name")]
    public string? CategoryName { get; set; }

    [BsonElement("subcats")]
    public List<Subcategory> Subcategories { get; set; } = new();
}
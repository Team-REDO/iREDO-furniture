using MongoDB.Bson.Serialization.Attributes;

namespace Subcategories;

[BsonIgnoreExtraElements]
public class Subcategory
{
    [BsonElement("subid")]
    public string? SubId { get; set; }

    [BsonElement("name")]
    public string? SubcategoryName { get; set; }
}
using Subcategories;
using MongoDB.Bson.Serialization.Attributes;

namespace Categories
{
    [BsonIgnoreExtraElements]
    public class Category
    {
        [BsonElement("catid")]
        public string? catId { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("subcats")]
        public List<Subcategory> Subcats { get; set; } = new();
    }
}
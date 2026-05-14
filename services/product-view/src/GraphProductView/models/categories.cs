using Subcategories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Categories
{
    [BsonIgnoreExtraElements]
    public class Category
    {
        [BsonId]
        public string _Id { get; set; }

        [BsonElement("id")]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set;}

        [BsonElement("subcat")]
        public Subcategory? subcat { get; set; }
    }
}
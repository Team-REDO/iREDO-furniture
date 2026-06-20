using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Colors
{
    [BsonIgnoreExtraElements]
    public class Color
    {
        [BsonElement("colorid")]
        public string? colorId { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("colorGuid")]
        public string ColorGuid { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("href")]
        public string? Href { get; set; }
    }
}
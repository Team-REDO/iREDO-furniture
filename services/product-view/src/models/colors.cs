
using MongoDB.Bson.Serialization.Attributes;

namespace Colors
{
    [BsonIgnoreExtraElements]
    public class Color
    {
        [BsonElement("id")]
        public string? Id { get; set; }=default;

        [BsonElement("name")]
        public string? Name { get; set; }=default;

        [BsonElement("href")]
        public string? Href { get; set; }=default;
    }
}
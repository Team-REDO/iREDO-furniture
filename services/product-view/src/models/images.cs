using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Images
{
    [BsonIgnoreExtraElements]
    public class Image
    {
        [BsonElement("imageId")]
        public string? ImageId { get; set; }

        // Business identifier (your domain identity)
        [BsonElement("imageGuid")]
        public string ImageGuid { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("url")]
        public string Url { get; set; } = string.Empty;
    }
}
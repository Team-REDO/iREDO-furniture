using MongoDB.Bson.Serialization.Attributes;

namespace Images;

[BsonIgnoreExtraElements]
public class Image
{
    [BsonElement("imageId")]
    public string? ImageId { get; set; }

    [BsonElement("imageGuid")]
    public string? ImageGuid { get; set; }

    [BsonElement("url")]
    public string? ImageUrl { get; set; }
}
using MongoDB.Bson.Serialization.Attributes;

namespace Colors;

[BsonIgnoreExtraElements]
public class Color
{
    [BsonElement("colorid")]
    public string? ColorId { get; set; }

    [BsonElement("colorGuid")]
    public string? ColorGuid { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("href")]
    public string? Href { get; set; }
}
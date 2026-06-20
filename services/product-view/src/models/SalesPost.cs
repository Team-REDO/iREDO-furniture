


using Categories;
using Colors;
using GraphProductView.Models;
using HotChocolate;
using Images;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Subcategories;
using models;

namespace Furnitures
{
public class SalesPost
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("sales_post_guid")]
    public Guid? SalesPostGuid { get; set; }

    [BsonElement("person_guid")]
    public Guid? PersonGuid { get; set; }

    [BsonElement("title")]
    public string? Title { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("size")]
    public string? Size { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("condition")]
    public ConditionType Condition { get; set; }

    [BsonElement("colors")]
    public List<Color> Colors { get; set; } = new List<Color>();

    [BsonElement("categories")]
    public List<Category>? Categories { get; set; } = new List<Category>();

    [BsonElement("images")]
    public List<Image>? Images { get; set; } = new List<Image>();
}
}


using models;

namespace Furnitures
{
public class SalesPost
{
    public string? Id { get; set; }

    public Guid? SalesPostGuid { get; set; }

    public Guid? PersonGuid { get; set; }
    public string? Title { get; set; }


    public string? Description { get; set; }
    public string? Size { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }
    public ConditionType Condition { get; set; }
    public List<Color> Colors { get; set; } = new List<Color>();
    public List<Category>? Categories { get; set; } = new List<Category>();
    public List<Image>? Images { get; set; } = new List<Image>();
}
}
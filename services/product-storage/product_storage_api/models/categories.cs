
namespace models
{
    public class Category
    {
        public string? Id { get; set; }=default;
        public string? Name { get; set;}=default;

        public List<Subcategory>? Subcats { get; set; }=default;
    }
}
namespace SynchronizerService.Models
{
    public class ListingDetails
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Size { get; set; } = ""; 

        public int Quantity { get; set; }
        public double Price { get; set; }

        public string Condition { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public List<Color> Colors { get; set; } = new();
        public List<SubCategory> SubCategories { get; set; } = new();
        public List<string> Images { get; set; } = new();
    }
}

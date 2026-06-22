namespace SynchronizerService.Models
{
    public class SubCategoryDb
    {
        public string Id { get; set; } // Mongo _id

        public string SubcategoryGuid { get; set; }
        public string SubcategoryName { get; set; }
    }
}
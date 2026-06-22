using System.Collections.Generic;

namespace SynchronizerService.Models
{
    public class CategoryDb
    {
        public string Id { get; set; } // Mongo _id
        public string CategoryGuid { get; set; }
        public string CategoryName { get; set; }

        public List<SubCategoryDb> Subcategories { get; set; } = new();
    }
}
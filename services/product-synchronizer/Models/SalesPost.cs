using System;
using System.Collections.Generic;

namespace SynchronizerService.Models
{
    public class SalesPost
    {
        public string Id { get; set; } // Mongo _id

        public string SalesPostGuid { get; set; }
        public string PersonGuid { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Condition { get; set; }
        public string City { get; set; }
        public DateTime ModifiedAt { get; set; }

        public List<ColorDb> Colors { get; set; } = new();
        public List<CategoryDb> Categories { get; set; } = new();
        public List<ImageDb> Images { get; set; } = new();
    }
}
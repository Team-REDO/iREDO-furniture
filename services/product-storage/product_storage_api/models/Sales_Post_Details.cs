using System.Drawing;

namespace models
{
    public class Sales_Post_Details
    {
        public string Id { get; set; }

        public string SalesPostId { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string Sales { get; set; }

        public string Titel { get; set; }

        public string Description { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public string City { get; set; }        
    }
}
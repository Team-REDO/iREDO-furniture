

namespace models
{
    public class Sales_Post_Removed
    {
        public string? Id { get; set; }
        public string? SalesPostId { get; set; }
        public bool removedAt { get; set; }
        public DateTime? Date { get; set; }
    }
}   
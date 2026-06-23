namespace models
{
    public class Sales_Posts
    {
        public string Id { get; set; }

        public Guid SalesPostGuid { get; set; }

        public Guid PersonGuid { get; set; }

        public DateTime createdAt { get; set; }
    }
}
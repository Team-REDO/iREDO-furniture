namespace UserService.DomainModels
{
    public class SavedListPost
    {
        public int Id { get; set; }

        public int SavedListId { get; set; }
        public SavedList SavedList { get; set; }

        public Guid SalesPostId { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
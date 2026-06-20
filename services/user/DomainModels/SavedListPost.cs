namespace UserService.DomainModels
{
    public class SavedListPost
    {
        public int SavedListId { get; set; }

        public SavedList SavedList { get; set; } = null!;

        public Guid SalesPostGuid { get; set; }
    }
}
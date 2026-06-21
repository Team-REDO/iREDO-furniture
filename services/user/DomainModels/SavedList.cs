namespace UserService.DomainModels
{
    public class SavedList
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; } = null!;

        public string Name { get; set; } = "";

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SavedListPost> Posts { get; set; } = new List<SavedListPost>();
    }
}
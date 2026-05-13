namespace UserService.DomainModels
{
    public class SavedList
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<SavedListPost> Posts { get; set; } = new List<SavedListPost>();
    }
}
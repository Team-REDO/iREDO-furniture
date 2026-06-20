namespace UserService.DomainModels
{
    public class Person
    {
        public int Id { get; set; }

        public Guid PersonGuid { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<PersonDetails> Details { get; set; } = new List<PersonDetails>();

        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public ICollection<SavedList> SavedLists { get; set; } = new List<SavedList>();

        public ICollection<PersonRemoved> RemovedRecords { get; set; } = new List<PersonRemoved>();
    }
}
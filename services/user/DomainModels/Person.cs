using UserService.DomainModels;
namespace UserService.DomainModels {
    public class Person
    {
        public int Id { get; set; }
        public PersonDetails Details { get; set; } = null!;
        public Guid Guid { get; set; }

        public ICollection<SavedList> SavedLists { get; set; } = new List<SavedList>();
        public ICollection<PersonRemoved> RemovedRecords { get; set; } = new List<PersonRemoved>();
    }
}
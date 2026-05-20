using user.DomainModels;
using UserService.DomainModels;
namespace UserService.DomainModels {
    public class Person
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public PersonDetails Details { get; set; } = null!;

        public Address? Address { get; set; }

        public ICollection<SavedList> SavedLists { get; set; } = new List<SavedList>();

        public ICollection<PersonRemoved> RemovedRecords { get; set; } = new List<PersonRemoved>();
    }
}
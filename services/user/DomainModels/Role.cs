using UserService.DomainModels;

namespace user.DomainModels
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<Person> Persons { get; set; } = new List<Person>();
    }
}

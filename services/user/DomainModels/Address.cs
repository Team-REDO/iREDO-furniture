using UserService.DomainModels;
namespace UserService.DomainModels
{

  
    public class Address
    {
        public int Id { get; set; }

        public int PersonDetailsId { get; set; }
        public PersonDetails PersonDetails { get; set; }

        public string? Street { get; set; }
        public string StreetNumber { get; set; }
        public string FloorDoor { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
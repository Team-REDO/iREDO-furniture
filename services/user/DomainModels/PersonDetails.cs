using System;
namespace UserService.DomainModels 
{
    public class PersonDetails
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person? Person { get; set; }

        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }

        public string? PhoneNumber { get; set; }
        public string Email { get; set; }

        public DateTime ModifiedDate { get; set; }
    }


}
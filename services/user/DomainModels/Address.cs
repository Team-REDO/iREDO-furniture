namespace UserService.DomainModels
{
    public class Address
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person? Person { get; set; }

        public string? Street { get; set; }

        public string StreetNumber { get; set; } = "";

        public string FloorDoor { get; set; } = "";

        public string ZipCode { get; set; } = "";

        public string City { get; set; } = "";

        public string Country { get; set; } = "";

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
namespace UserService.DomainModels
{
    public class PersonRemoved
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person? Person { get; set; }

        public DateTime RemovedAt { get; set; }
    }
}
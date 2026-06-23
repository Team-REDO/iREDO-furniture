using UserService.DomainModels;

namespace user.DTOs
{
    public class DetailsRequestDto
    {
        public string? Firstname { get; set; }

        public string? Middlename { get; set; }

        public string? Lastname { get; set; }

        public string? PhoneNumber { get; set; }
    }
}

namespace user.DTOs
{
    public class MeResponseDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid PersonGuid { get; set; }

        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

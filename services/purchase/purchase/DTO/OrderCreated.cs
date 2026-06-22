using Purchase.Enums;
using Purchase.Models;

namespace DTO
{
    public class OrderCreated
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = string.Empty;

        public List<string> SalesPostGuid { get; set; }= new List<string>();

        public string Email { get; set; } = string.Empty;
    }
}
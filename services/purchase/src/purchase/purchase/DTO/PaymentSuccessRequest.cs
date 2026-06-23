namespace DTO;

public class PaymentSuccessRequest
{
    public string Email { get; set; } = string.Empty;
    public string OrderGuid { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
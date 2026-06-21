using System.Text.Json;
namespace EmailService.Models;

public class EmailMessage
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;

    public JsonElement Body { get; set; } 
}
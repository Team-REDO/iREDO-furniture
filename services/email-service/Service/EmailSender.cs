using MailKit.Net.Smtp;
using MimeKit;

namespace EmailService.Services;

public class EmailSender
{
    public void Send(string to, string subject, string body)
    {
        var apiKey = Environment.GetEnvironmentVariable("MAILJET_API_KEY");
        var secret = Environment.GetEnvironmentVariable("MAILJET_SECRET");

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secret))
        {
            Console.WriteLine("Mailjet credentials missing");
            return;
        }

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("RabbitMQ Service", "kamikazi68@gmail.com"));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        try
        {
            Console.WriteLine("Connecting to SMTP...");

            client.Connect(
                "in-v3.mailjet.com",
                587,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            Console.WriteLine("Authenticating...");

            client.Authenticate(apiKey, secret);

            Console.WriteLine("Sending email...");

            client.Send(message);

            Console.WriteLine($"Email sent to {to}");

            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("SMTP ERROR:");
            Console.WriteLine(ex.Message);
        }
    }
}
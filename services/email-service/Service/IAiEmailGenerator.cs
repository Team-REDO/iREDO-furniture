public interface IAiEmailGenerator
{
    Task<string> GenerateEmail(string subject, string context);
}
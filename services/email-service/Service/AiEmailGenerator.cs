using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class AiEmailGenerator
{
    private readonly HttpClient _http;

    public AiEmailGenerator()
    {
        _http = new HttpClient();
        
        var apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");
       

        Console.WriteLine("ENV KEY: " + Environment.GetEnvironmentVariable("OPENROUTER_API_KEY"));
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("OPENROUTER_API_KEY is not set");
        }

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        _http.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost");
        _http.DefaultRequestHeaders.Add("X-Title", "EmailService");
    }

    public async Task<string> GenerateEmail(string subject, string context)
    {
        var prompt = $"Write a short, friendly customer email.\nSubject: {subject}\nDetails: {context}";

        var requestBody = new
        {
            //model = "meta-llama/llama-3-8b-instruct",
            model = "openrouter/free",

            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var response = await _http.PostAsync(
            "https://openrouter.ai/api/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        );

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        return doc
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "Fallback email content";
    }
}
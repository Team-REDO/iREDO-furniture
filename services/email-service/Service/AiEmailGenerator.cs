using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class AiEmailGenerator : IAiEmailGenerator
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

    public async Task<string> GenerateEmail(string subject, string context)  // <-- This is the prompt we send to the AI model that determines how it behaves
    {
        var prompt = $@" 
        You are a customer support assistant for an e-commerce company.

        Write a short confirmation email after a purchase.

        Include:
        - A friendly greeting
        - Confirmation of purchase
        - Mention the product briefly
        - A closing sentence
        - include a list of all items purchased with their names and prices
        - include a total price at the end of the list

        Tone: Friendly, human, slightly enthusiastic

        Subject: {subject}
        Details: {context}";

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
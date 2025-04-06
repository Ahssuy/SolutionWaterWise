using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json;

namespace WaterFootprint
{
    public class GeminiService : IGeminiService
    {
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        private const string SystemInstruction = @"
You are an expert in agricultural irrigation systems and water resource management. 
Your role is to analyze sensor data from irrigation machines to help determine optimal irrigation needs for different parcels of land. 
Based on the prediction and its meaning, provide accurate, easy-to-understand explanations tailored for farmers and agricultural workers.

Additionally, act as a Water Footprint Coach. Recommend practical, sustainable strategies to reduce water usage while maintaining crop health and yield. 
Suggest eco-friendly practices, modern irrigation techniques, or behavioral changes to minimize water footprint on farms.

Always respond clearly, supportively, and with helpful, actionable advice. If the user asks follow-up questions, continue the conversation naturally and with expertise.";

        public async Task<string> AskGeminiAsync(string question, string context)
        {
            string apiKey = await SecureStorage.Default.GetAsync("GeminiApiKey");

            if (string.IsNullOrEmpty(apiKey))
                return "API key not found. Please set it in SecureStorage.";

            string endpoint = $"{BaseUrl}?key={apiKey}";

            using var client = new HttpClient();

            // Combine system instructions with user input
            var fullPrompt = $"{SystemInstruction}\n\nPrediction: {context}\n\nQuestion: {question}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = fullPrompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.75,
                    maxOutputTokens = 1024,
                    responseMimeType = "text/plain"
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine("Gemini raw response:\n" + responseBody);

            if (!response.IsSuccessStatusCode)
            {
                return $"Gemini Error: {response.StatusCode} - {responseBody}";
            }

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var candidates = doc.RootElement.GetProperty("candidates");

                if (candidates.ValueKind == JsonValueKind.Array && candidates.GetArrayLength() > 0)
                {
                    var contentJson = candidates[0].GetProperty("content");
                    var parts = contentJson.GetProperty("parts");

                    if (parts.ValueKind == JsonValueKind.Array && parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString();
                        return text ?? "Gemini returned an empty response.";
                    }
                }

                return "Gemini response did not contain any text.";
            }
            catch (Exception ex)
            {
                return $"Parsing error: {ex.Message}";
            }
        }
    }
}

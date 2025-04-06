
namespace WaterFootprint
{
    public interface IGeminiService
    {
        Task<string> AskGeminiAsync(string question, string context);
    }
}
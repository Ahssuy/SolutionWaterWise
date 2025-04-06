using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace WaterFootprint
{
    public partial class GeminiPage : ContentPage
    {
        private readonly IGeminiService _geminiService;
        private readonly string _context;

        public GeminiPage(string prediction, string meaning, IGeminiService geminiService)
        {
            InitializeComponent();
            _geminiService = geminiService;
            _context = $"Prediction: {prediction}\nMeaning: {meaning}";

            AddMessage("Gemini", $"Hello! Ask me anything about {prediction} or irrigation.", true);
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            string question = UserInputEntry.Text?.Trim();
            if (string.IsNullOrEmpty(question)) return;

            AddMessage("You", question, false);
            UserInputEntry.Text = string.Empty;

            AddMessage("Gemini", "Thinking...", true);

            try
            {
                string response = await _geminiService.AskGeminiAsync(question, _context);

                // Replace "Thinking..." with actual answer
                ChatStack.Children.RemoveAt(ChatStack.Children.Count - 1);
                AddMessage("Gemini", response, true);
            }
            catch (Exception ex)
            {
                ChatStack.Children.RemoveAt(ChatStack.Children.Count - 1);
                AddMessage("Gemini", $"Error: {ex.Message}", true);
            }

            await Task.Delay(100);
            ChatScrollView.ScrollToAsync(ChatStack, ScrollToPosition.End, true);
        }

        private void AddMessage(string sender, string message, bool isBot)
        {
            ChatStack.Children.Add(new Frame
            {
                Padding = 10,
                BackgroundColor = isBot ? Color.FromArgb("#E0F7FA") : Color.FromArgb("#BBDEFB"),
                CornerRadius = 12,
                HorizontalOptions = isBot ? LayoutOptions.Start : LayoutOptions.End,
                Content = new Label
                {
                    Text = $"{sender}: {message}",
                    FontSize = 14,
                    TextColor = Colors.Black
                }
            });
        }
    }
}

using WaterFootprint;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace WaterFootprint
{
    public partial class PredictionPage : ContentPage
    {
        private readonly PredictionService _predictionService;

        public PredictionPage()
        {
            InitializeComponent();
            _predictionService = new PredictionService();
        }

        private async void OnPredictClicked(object sender, EventArgs e)
        {
            try
            {
                var inputs = new List<float>
                {
                    float.Parse(Sensor1.Text),
                    float.Parse(Sensor2.Text),
                    float.Parse(Sensor3.Text),
                    float.Parse(Sensor4.Text),
                    float.Parse(Sensor5.Text),
                    float.Parse(Sensor6.Text),
                    float.Parse(Sensor7.Text),
                    float.Parse(Sensor8.Text),
                    float.Parse(Sensor9.Text),
                    float.Parse(Sensor10.Text),
                    float.Parse(Sensor11.Text),
                    float.Parse(Sensor12.Text),
                    float.Parse(Sensor13.Text),
                    float.Parse(Sensor14.Text),
                    float.Parse(Sensor15.Text),
                    float.Parse(Sensor16.Text),
                    float.Parse(Sensor17.Text),
                    float.Parse(Sensor18.Text),
                    float.Parse(Sensor19.Text),
                    float.Parse(Sensor20.Text)
                };

                string result = await _predictionService.PredictAsync(inputs);

                string meaning = result switch
                {
                    "0" => "Parcel 0 likely needs irrigation soon due to low moisture levels.",
                    "1" => "Parcel 1 is moderately irrigated. Continue monitoring.",
                    "2" => "Parcel 2 is sufficiently irrigated. No action needed.",
                    _ => "Unable to interpret the prediction result."
                };

                // Show prediction in an alert
                bool askGemini = await DisplayAlert(
                    "Prediction Result",
                    $"Predicted Parcel: {result}\n\nMeaning: {meaning}\n\nWould you like to ask Gemini AI more about this?",
                    "Ask Gemini",
                    "Close"
                );

                // Navigate to Gemini chatbot page if user chooses
                if (askGemini)
                {
                    var geminiService = new GeminiService(); // Replace with real API integration later
                    await Navigation.PushAsync(new GeminiPage(result, meaning, geminiService));
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Text = $"Error: {ex.Message}";
            }
        }
    }
}

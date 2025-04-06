using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using WaterFootprint;

namespace WaterFootprint
{
    public class PredictionService
    {
        private readonly string projectId = "irrigationsensor09";
        private readonly string endpointId = "5918306054457786368";
        private readonly string region = "asia-south1";

        public async Task<string> PredictAsync(List<float> sensorData)
        {
            var accessToken = await GCPAuthHelper.GetAccessTokenAsync();

            string url = $"https://asia-south1-aiplatform.googleapis.com/v1/projects/irrigationsensor09/locations/asia-south1/endpoints/5918306054457786368:predict";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestBody = new
            {
                instances = new List<List<float>> { sensorData }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);
            var prediction = doc.RootElement
                                .GetProperty("predictions")[0]
                                .GetInt32();

            return prediction.ToString();

        }
    }
}

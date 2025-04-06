using System.Reflection;
using System.IO;
using Google.Apis.Auth.OAuth2;

public static class GCPAuthHelper
{
    public static async Task<string> GetAccessTokenAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "WaterFootprint.Resources.service_account.json"; 

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new(stream);
        string json = await reader.ReadToEndAsync();

        var credential = GoogleCredential
            .FromJson(json)
            .CreateScoped("https://www.googleapis.com/auth/cloud-platform");

        return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
    }
}

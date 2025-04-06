namespace WaterFootprint
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetGeminiApiKey();

            MainPage = new AppShell();
        }

        private async void SetGeminiApiKey()
        {
#if DEBUG
            
            var existingKey = await SecureStorage.Default.GetAsync("GeminiApiKey");
            if (string.IsNullOrEmpty(existingKey))
            {
                //await SecureStorage.Default.SetAsync("GeminiApiKey");
            }
#endif
        }
    }

}

namespace WaterFootprint
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GeminiPage), typeof(GeminiPage));
        }

        public MainPage MainPage { get; }
    }
}

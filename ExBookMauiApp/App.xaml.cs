using Microsoft.Extensions.Caching.Memory;

namespace ExBookMauiApp
{
    public partial class App : Application
    {
        private static readonly Lazy<HttpClient> lazyHttpClient = new Lazy<HttpClient>(() =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ConnectionString)
            };
            return client;
        });

        public static readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        public static string ConnectionString = "https://3e9d-2a00-f3c-7060-0-348f-4684-bd9f-a28f.ngrok-free.app";
        public static HttpClient httpClient => lazyHttpClient.Value;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            // SecureStorage.RemoveAll();
            Routing.RegisterRoute("Authorization", typeof(ExBookMauiApp.Pages.Authorization));
            Routing.RegisterRoute("Registration", typeof(ExBookMauiApp.Pages.Registration));
            Routing.RegisterRoute("Settings", typeof(ExBookMauiApp.Pages.Settings));
            Authorized();
        }

        public async Task Authorized()
        {
            string token = await SecureStorage.Default.GetAsync("refreshToken");

            if (token == null)
                await Shell.Current.GoToAsync("Authorization");
            else
                await Shell.Current.GoToAsync("//pages/Home");
        }
    }
}

namespace ExBookMauiApp
{
    public partial class App : Application
    {
        public static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://dce3-46-130-163-56.ngrok-free.app") };

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            //SecureStorage.RemoveAll();
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

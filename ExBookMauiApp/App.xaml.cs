using System.Text;
using ExBookMauiApp.Models.UserModels;

namespace ExBookMauiApp
{
    public partial class App : Application
    {
        public static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://775c-46-71-108-172.ngrok-free.app") };

        public  App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Routing.RegisterRoute("Authorization", typeof(ExBookMauiApp.Pages.Authorization));
            Routing.RegisterRoute("Registration", typeof(ExBookMauiApp.Pages.Registration));
            Routing.RegisterRoute("Settings", typeof(ExBookMauiApp.Pages.Settings));
            Authorized();
        }
        public async Task Authorized()
        {
            string token = await SecureStorage.GetAsync("refreshToken");

            if (token == null)
                await Shell.Current.GoToAsync("Authorization");
            else
                await Shell.Current.GoToAsync("//pages/Home");
        }

    }
}

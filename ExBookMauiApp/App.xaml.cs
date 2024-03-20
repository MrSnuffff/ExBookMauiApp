namespace ExBookMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Routing.RegisterRoute("Authorization", typeof(ExBookMauiApp.Pages.Authorization));
            Routing.RegisterRoute("Registration", typeof(ExBookMauiApp.Pages.Registration));
            Routing.RegisterRoute("Settings", typeof(ExBookMauiApp.Pages.Settings));

            Authorized();
        }
        public async void Authorized()
        {
            Shell.Current.GoToAsync("Authorization");
        }
    }
}

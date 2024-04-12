using ExBookMauiApp.Models.UserModels;
using ExBookMauiApp.HttpRequests;
using ExBookMauiApp.Resources.Strings;
namespace ExBookMauiApp.Pages;

public partial class Authorization : ContentPage
{
	public Authorization()
	{
		InitializeComponent(); 


    }

    private async void CreateAnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Registration");
    }

   
    private async void SignIn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(username.Text))
        {
            username.Placeholder = "Username is required";
            username.PlaceholderColor = Colors.Red;
            return;
        }
        if (string.IsNullOrEmpty(passwordHash.Text))
        {
            passwordHash.Placeholder = "Password is required";
            passwordHash.PlaceholderColor = Colors.Red;
            return;
        }
        LoginModel loginModel = new LoginModel { email = username.Text, passwordHash = passwordHash.Text };
        string accessToken, refreshToken;
        (accessToken, refreshToken) = await  AuthorizationRequests.LoginRequestViaEmail(loginModel);
         
        if (accessToken != null || refreshToken != null)
        {
            await SecureStorage.Default.SetAsync("accessToken", accessToken);
            await SecureStorage.Default.SetAsync("refreshToken", refreshToken);
            await Shell.Current.GoToAsync("//pages/Home");
        }
            
        else
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = AppResources.WrongPasswordOrUserName;
        }
    }
   
    

    private async void ForgotPassword_Clicked(object sender, TappedEventArgs e)
    {

    }
}
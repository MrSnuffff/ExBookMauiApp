using ExBookMauiApp.Models.UserModels;
using Newtonsoft.Json.Linq;
using System.Text;
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
        bool succsesLogin = await LoginRequest(loginModel);
        if (succsesLogin)
            await Shell.Current.GoToAsync("//pages/Home");
        else
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = AppResources.WrongPasswordOrUserName;
        }
    }
    static async Task<bool> LoginRequest(LoginModel loginModel)
    {
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
        byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
        HttpContent content = new ByteArrayContent(contentBytes);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await App.httpClient.PostAsync("api/Users/LoginViaEmail", content);
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject token = JObject.Parse(responseBody);
            string accessToken = token["accessToken"].ToString();
            string refreshToken = token["refreshToken"].ToString();
            SecureStorage.RemoveAll();
            await SecureStorage.SetAsync("accessToken", accessToken);
            await SecureStorage.SetAsync("refreshToken", refreshToken);


            return true;
        }
        else
        {
            return false;
        }
    }

    private async void ForgotPassword_Clicked(object sender, TappedEventArgs e)
    {

    }
}
using ExBookMauiApp.Models.UserModels;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ExBookMauiApp.Pages;

public partial class Registration : ContentPage
{
    public int remainingTimeInSeconds = 300;


    public Registration()
	{
		InitializeComponent();
        UsernameEmailLayout.IsVisible = true;
        LastFirstNameLayout.IsVisible = false;
        PhoneNumberLayout.IsVisible = false;
        PasswordLayout.IsVisible = false;
        SendEmailLayout.IsVisible = false;
    }
    private void UsernameEmailNextButton(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(username.Text))
        {
            username.Placeholder = "Username is required";
            username.PlaceholderColor = Colors.Red;
        }
        if (string.IsNullOrEmpty(emailEntry.Text))
        {
            emailEntry.Placeholder = "Email is required";
            emailEntry.PlaceholderColor = Colors.Red;
        }
        if(!string.IsNullOrEmpty(username.Text) && !string.IsNullOrEmpty(emailEntry.Text))
        {
            UsernameEmailLayout.IsVisible = false;
            LastFirstNameLayout.IsVisible = true;
        }
        
    }
    private void LastFirstNameNextButton(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(lastname.Text))
        {
            lastname.Placeholder = "Last name is required";
            lastname.PlaceholderColor = Colors.Red;
        }
        if (string.IsNullOrEmpty(firstname.Text))
        {
            firstname.Placeholder = "First name is required";
            firstname.PlaceholderColor = Colors.Red;
        }
        if(!string.IsNullOrEmpty(lastname.Text) && !string.IsNullOrEmpty(firstname.Text))
        {
            LastFirstNameLayout.IsVisible = false;
            PhoneNumberLayout.IsVisible = true;
            ErrorLabel.IsVisible = false;
        }
    }
    private void LastFirstNameBackButton(object sender, EventArgs e)
    {

        UsernameEmailLayout.IsVisible = true;
        LastFirstNameLayout.IsVisible = false;
    }
    private void PhoneNumberNextButton(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(phonenumber.Text))
        {
            phonenumber.Placeholder = "Phone number is required";
            phonenumber.PlaceholderColor = Colors.Red;
        }
        else
        {
            PhoneNumberLayout.IsVisible = false;
            PasswordLayout.IsVisible = true;

            PasswordNextButtonName.IsEnabled = true;
        }
    }
    private void PhoneNumberBackButton(object sender, EventArgs e)
    {
        PhoneNumberLayout.IsVisible = false;
        LastFirstNameLayout.IsVisible = true;
    }
    private void PasswordBackButton(object sender, EventArgs e)
    {
        PasswordLayout.IsVisible = false;
        PhoneNumberLayout.IsVisible = true;
    }
    private async void PasswordNextButton(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(password.Text))
        {
            password.Placeholder = "password  is required";
            password.PlaceholderColor = Colors.Red;
        }
        if (string.IsNullOrEmpty(confirmpassword.Text))
        {
            confirmpassword.Placeholder = "Confirm password is required";
            confirmpassword.PlaceholderColor = Colors.Red;
        }
        if (!string.IsNullOrEmpty(lastname.Text) && !string.IsNullOrEmpty(firstname.Text))
        {
            if(password.Text == confirmpassword.Text)
            {
                PasswordNextButtonName.IsEnabled = false;
                RegistrationModel registrationModel = new RegistrationModel()
                {
                    username = username.Text,
                    first_name = firstname.Text,
                    last_name = lastname.Text,
                    email = emailEntry.Text,
                    phone_number = phonenumber.Text,
                    passwordHash = password.Text
                };
                bool succsesLogin = await RegistrationRequestAsync(registrationModel);
                if (succsesLogin)
                {
                    PasswordLayout.IsVisible = false;
                    SendEmailLayout.IsVisible = true;
                    StartTimer();
                }
                else
                {
                    PasswordLayout.IsVisible = false;
                    UsernameEmailLayout.IsVisible = true;
                    ErrorLabel.IsVisible = true;
                }
            }
        }
    }
    public async Task<bool> RegistrationRequestAsync(RegistrationModel registrationModel)
    {
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(registrationModel);
        byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
        HttpContent content = new ByteArrayContent(contentBytes);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await App.httpClient.PostAsync("api/Users/Registration", content);
        if (response.IsSuccessStatusCode)
        {
            EmailModel email = new EmailModel{ email = emailEntry.Text };
            await SendVerifyCode(email);
            return true;
        }
        else
        {
            return false;
        }
    }
    public async void ConfirmEmailButton(object sender, EventArgs e)
    {
        ConfirmEmail confirmEmail = new ConfirmEmail { email = emailEntry.Text, confirmCode = confirmCode.Text };
        bool succsesLogin = await SendCheckVerifyCode(confirmEmail);
        if (succsesLogin)
            await Shell.Current.GoToAsync("//pages/Home");
        else
        {
            confirmCode.Text = "";
            confirmCode.Placeholder = "error try again";
            confirmCode.PlaceholderColor = Colors.Red;
        }
    }
    public async Task SendVerifyCode(EmailModel email)
    {
        remainingTimeInSeconds = 300;
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(email);
        byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
        HttpContent content = new ByteArrayContent(contentBytes);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await App.httpClient.PostAsync("api/Users/SendVerifyCode", content);
    }
    public async Task<bool> SendCheckVerifyCode(ConfirmEmail confirmEmail)
    {
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(confirmEmail);
        byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
        HttpContent content = new ByteArrayContent(contentBytes);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await App.httpClient.PostAsync("api/Users/CheckVerifyCode", content);
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
            return false;
    }
    private async void ResendVerifyCode(object sender, EventArgs e)
    {
        EmailModel email = new EmailModel { email = emailEntry.Text };
        await SendVerifyCode(email);
    }
    private void UpdateTimerLabel()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTimeInSeconds);
        timerLabel.Text = $"{timeSpan:mm\\:ss}";
    }
    private async void StartTimer()
    {
        while (remainingTimeInSeconds > 0)
        {
            await Task.Delay(1000);
            remainingTimeInSeconds--;
            UpdateTimerLabel();
        }
    }
}
using ExBookMauiApp.Models.UserModels;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ExBookMauiApp.HttpRequests
{

    public class AuthorizationRequests
    {
        

        public static async Task<(string, string)> LoginRequestViaEmail(LoginModel loginModel)
        {
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
            byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
            HttpContent content = new ByteArrayContent(contentBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await App.httpClient.PostAsync("api/Authentication/LoginViaEmail", content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject token = JObject.Parse(responseBody);
                string accessToken = token["accessToken"].ToString();
                string refreshToken = token["refreshToken"].ToString();
                
                return (accessToken, refreshToken);
            }
            else
            {
                return (null, null);
            }
        }

        public static async Task<bool> RegistrationRequestAsync(RegistrationModel registrationModel)
        {
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(registrationModel);
            byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
            HttpContent content = new ByteArrayContent(contentBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await App.httpClient.PostAsync("api/Authentication/Registration", content);
            if (response.IsSuccessStatusCode)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task SendVerifyCode(EmailModel email)
        {
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(email);
            byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
            HttpContent content = new ByteArrayContent(contentBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await App.httpClient.PostAsync("api/Authentication/SendVerifyCode", content);
        }


        public static async Task<(string, string)> CheckVerifyCode(ConfirmEmail confirmEmail)
        {
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(confirmEmail);
            byte[] contentBytes = Encoding.UTF8.GetBytes(jsonData);
            HttpContent content = new ByteArrayContent(contentBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await App.httpClient.PostAsync("api/Authentication/CheckVerifyCode", content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject token = JObject.Parse(responseBody);
                string accessToken = token["accessToken"].ToString();
                string refreshToken = token["refreshToken"].ToString();
               
                return (accessToken, refreshToken);
            }
            else
                return (null, null);
        }
        public static async Task<string> RefreshToken()
        {
            string refreshToken = await SecureStorage.Default.GetAsync("refreshToken");
            HttpResponseMessage response = await App.httpClient.PostAsync($"api/Authentication/RefreshToken?refreshToken={refreshToken}", null);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject token = JObject.Parse(responseBody);
                string accessToken = token["accessToken"].ToString();
                await SecureStorage.Default.SetAsync("accessToken", accessToken);
                return accessToken;
            }
            else
            {

                SecureStorage.Remove("refreshToken");
                await Shell.Current.GoToAsync("Authorization"); 

                return null;
            }

               
        }
    }
}

using ExBookMauiApp.Models.UserModels;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ExBookMauiApp.HttpRequests
{
    public class ExBookWebApiRequests
    {
        public static async Task<ProfileModel> GetUserProfile(string accsessToken)
        {
            HttpResponseMessage response = await App.httpClient.PostAsync($"api/Users/GetUserProfile?accessToken={accsessToken}", null);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject profileJson = JObject.Parse(responseBody);
                ProfileModel profileModel = new ProfileModel
                {
                    username = profileJson["username"].ToString(),
                    last_name = profileJson["last_name"].ToString(),
                    first_name = profileJson["first_name"].ToString(),
                    email = profileJson["email"].ToString(),
                    phone_number = profileJson["phone_number"].ToString(),
                    coins = ((int?)profileJson["coins"]),
                    photo = profileJson["photo"]?.ToObject<byte[]>()
                };
                return profileModel;
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string refreshToken = await SecureStorage.Default.GetAsync("refreshToken");

                string accessToken = await AuthorizationRequests.RefreshToken(refreshToken);
                if(accessToken != null)
                    return await GetUserProfile(accessToken);
            }
            return null;
        }

    }
}

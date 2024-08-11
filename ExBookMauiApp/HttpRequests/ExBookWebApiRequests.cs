using ExBookMauiApp.Models.UserModels;
using ExBookMauiApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;

namespace ExBookMauiApp.HttpRequests
{
    public class ExBookWebApiRequests
    {
        private static async Task<HttpResponseMessage> SendRequestWithTokenRefresh(Func<string, Task<HttpResponseMessage>> httpRequestFunc)
        {
            string accessToken = await SecureStorage.Default.GetAsync("accessToken");
            HttpResponseMessage response = await httpRequestFunc(accessToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                
                string newAccessToken = await AuthorizationRequests.RefreshToken();

                if (!string.IsNullOrEmpty(newAccessToken))
                {
                    await SecureStorage.Default.SetAsync("accessToken", newAccessToken);
                    response = await httpRequestFunc(newAccessToken);
                }
            }

            return response;
        }
        public static async Task GetUserBooks()
        {
            try
            {
                HttpResponseMessage response = await SendRequestWithTokenRefresh(async (token) =>
                {
                    return await App.httpClient.PostAsync($"api/Userbook/GetAllUserBooks?accessToken={token}", null);
                });

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var books = JsonConvert.DeserializeObject<List<Userbook>>(responseBody) ?? new List<Userbook>();
                    App.memoryCache.Set("UserBooksCache", books, TimeSpan.FromDays(30));
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static async Task GetUserProfile()
        {
            try
            {
                HttpResponseMessage response = await SendRequestWithTokenRefresh(async (token) =>
                {
                    return await App.httpClient.PostAsync($"api/Users/GetUserProfile?accessToken={token}", null);
                });

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject profileJson = JObject.Parse(responseBody);
                    ProfileModel profile = new ProfileModel()
                    {
                        username = profileJson["username"]?.ToString(),
                        last_name = profileJson["last_name"]?.ToString(),
                        first_name = profileJson["first_name"]?.ToString(),
                        email = profileJson["email"]?.ToString(),
                        phone_number = profileJson["phone_number"]?.ToString(),
                        coins = (int?)profileJson["coins"],
                        photo_url = profileJson["photo_url"]?.ToString(),
                    };
                    string userProfileJson = JsonConvert.SerializeObject(profile);
                    await SecureStorage.SetAsync("UserProfile", userProfileJson);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public static async Task AddUserPhoto(FileResult result)
        {
            try
            {
                var stream = await result.OpenReadAsync();
                var content = new MultipartFormDataContent
                {
                    { new StreamContent(stream) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } }, "photo", result.FileName }
                };

                HttpResponseMessage response = await SendRequestWithTokenRefresh(async (token) =>
                {
                    return await App.httpClient.PostAsync($"api/users/uploadphoto?accessToken={token}", content);
                });

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jobject = JObject.Parse(responseBody);

                    string userProfileJson = await SecureStorage.GetAsync("UserProfile");
                    ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(userProfileJson);
                    profile.photo_url = jobject["PhotoUrl"]?.ToString();

                    userProfileJson = JsonConvert.SerializeObject(profile);
                    await SecureStorage.SetAsync("UserProfile", userProfileJson);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}

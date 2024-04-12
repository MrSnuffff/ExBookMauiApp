using ExBookMauiApp.HttpRequests;
using ExBookMauiApp.Models.UserModels;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExBookMauiApp.Pages;

public partial class Profile : ContentPage
{
	public Profile()
	{
		InitializeComponent();
        OnStart();

    }
    public async void OnStart()
    {

        string ProfileJson = await SecureStorage.Default.GetAsync("UserProfile");

        if (!string.IsNullOrEmpty(ProfileJson))
            UpdateProfileFields();
        
        else
        {
            string accessToken = await SecureStorage.Default.GetAsync("accessToken");
            ProfileModel profile = await ExBookWebApiRequests.GetUserProfile(accessToken);
            string userProfileJson = JsonConvert.SerializeObject(profile);
            await SecureStorage.SetAsync("UserProfile", userProfileJson);

            UpdateProfileFields();
        }
    }


    public async void UpdateProfileFields()
    {

        string ProfileJson = await SecureStorage.Default.GetAsync("UserProfile");
        ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(ProfileJson);

        username.Text = profile.username;
        coin.Text = profile.coins.ToString();
        if (profile.photo != null)
        {
            byte[] byteArray = profile.photo;
            var image = ImageSource.FromStream(() => new MemoryStream(byteArray));
            photo.Source = image;
        }
    }
}
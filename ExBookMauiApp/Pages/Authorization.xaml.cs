using System.Text;

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
        
    }

    

    private async void ForgotPassword_Clicked(object sender, TappedEventArgs e)
    {

    }
}
namespace ExBookMauiApp.Pages;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
        UsernameEmailLayout.IsVisible = true;
        LastFirstNameLayout.IsVisible = false;
        PhoneNumberLayout.IsVisible = false;
        PasswordLayout.IsVisible = false;

    }
    private void UsernameEmailNextButton(object sender, EventArgs e)
    {
        UsernameEmailLayout.IsVisible = false;
        LastFirstNameLayout.IsVisible = true;
    }
    private void LastFirstNameNextButton(object sender, EventArgs e)
    {
        LastFirstNameLayout.IsVisible = false;
        PhoneNumberLayout.IsVisible = true;
    }
    private void LastFirstNameBackButton(object sender, EventArgs e)
    {
        UsernameEmailLayout.IsVisible = true;
        LastFirstNameLayout.IsVisible = false;
    }
    private void PhoneNumberNextButton(object sender, EventArgs e)
    {
        PhoneNumberLayout.IsVisible = false;
        PasswordLayout.IsVisible = true;
    }
    private void PhoneNumberBackButton(object sender, EventArgs e)
    {
        PhoneNumberLayout.IsVisible = false;
        LastFirstNameLayout.IsVisible = true;
    }
    private void PasswordNextButton(object sender, EventArgs e)
    {
        
    }
    private void PasswordBackButton(object sender, EventArgs e)
    {
        PasswordLayout.IsVisible = false;
        PhoneNumberLayout.IsVisible = true;
    }
}
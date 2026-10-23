namespace CustomGarmentApp.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void CreateClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }

    private async void LoginClicked(object sender, EventArgs e)
    {
        // Temporary navigation
        // Authentication will be implemented later.
        await Shell.Current.GoToAsync("GarmentPage");
    }

}
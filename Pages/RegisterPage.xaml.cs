namespace CustomGarmentApp.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}
    private async void BackClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

}
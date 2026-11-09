using CustomGarmentApp.Services;

namespace CustomGarmentApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    // Navigates to the registration page.
    private async void CreateClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }

    // Authenticates users against the SQLite database.
    private async void LoginClicked(object? sender, EventArgs e)
    {

        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";

        // Ensure both login fields contain information.
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please enter your email and password.",
                "OK");
            return;
        }

        try
        {
            UserDatabase database = new UserDatabase();

            // Verify the email and password against saved credentials.
            bool authenticated = await database.LoginAsync(
                email, password);

            if (!authenticated)
            {
                await DisplayAlertAsync(
                    "Login Failed",
                    "Incorrect email or password.",
                    "OK");
                return;
            }


            // Determine the account role only after authentication succeeds.
            bool isAdmin = await database.IsAdministratorAsync(email);


            // Start an authenticated session after verifying
            // the user's password and administrator role.
            SessionService.StartSession(email, isAdmin);


            if (isAdmin)
            {
                await DisplayAlertAsync(
                    "Administrator Login",
                    "Welcome! You are signed in as an administrator.",
                    "OK");
            }
            else
            {
                await DisplayAlertAsync(
                    "Customer Login",
                    "Welcome! You are signed in as a customer.",
                    "OK");
            }



            // Clear sensitive information after successful login.
            PasswordEntry.Text = "";

            await DisplayAlertAsync(
                "Login Successful",
                "Welcome back!",
                "OK");

            // Navigate to the garment page after authentication.
            await Shell.Current.GoToAsync("GarmentPage");
        }
        catch (Exception)
        {
            await DisplayAlertAsync(
                "Login Error",
                "Unable to sign in. Please try again.",
                "OK");
        }
    }


    // Opens the password management screen.
    private async void ChangePasswordClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChangePasswordPage");
    }
}

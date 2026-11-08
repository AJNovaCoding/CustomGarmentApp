using CustomGarmentApp.Services;
using System.Text.RegularExpressions;
namespace CustomGarmentApp.Pages;


public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    // Validates registration information when the user clicks Create Account.
    private async void RegisterClicked(object sender, EventArgs e)
    {
        string fullName = FullNameEntry.Text?.Trim() ?? "";
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";
        string confirmPassword = ConfirmPasswordEntry.Text ?? "";

        // Ensure all required fields are completed.
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlertAsync("Error", "Please complete all fields.", "OK");
            return;
        }

        // Validate email format.
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlertAsync("Error", "Please enter a valid email address.", "OK");
            return;
        }

        // Enforce password complexity requirements.
        bool validPassword =
            password.Length >= 8 &&
            Regex.IsMatch(password, @"[A-Z]") &&
            Regex.IsMatch(password, @"[a-z]") &&
            Regex.IsMatch(password, @"[0-9]") &&
            Regex.IsMatch(password, @"[^a-zA-Z0-9]");

        if (!validPassword)
        {
            await DisplayAlertAsync(
                "Invalid Password",
                "Password must contain at least 8 characters, " +
                "one uppercase letter, one lowercase letter, " +
                "one number, and one special character.",
                "OK");
            return;
        }

        // Ensure the password and confirmation match.
        if (password != confirmPassword)
        {
            await DisplayAlertAsync(
                "Error",
                "Passwords do not match.",
                "OK");
            return;
        }

        // Save the validated user account to SQLite.
        UserDatabase database = new UserDatabase();

        try
        {
            bool registered = await database.RegisterAsync(
                fullName, email, password);

            if (!registered)
            {
                await DisplayAlertAsync(
                    "Account Exists",
                    "An account with this email already exists.",
                    "OK");
                return;
            }

            // Clear password fields after successful registration.
            PasswordEntry.Text = "";
            ConfirmPasswordEntry.Text = "";

            await DisplayAlertAsync(
                "Registration Successful",
                "Your account has been created! Please sign in.",
                "OK");

            // Navigate back to the login page.
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception)
        {
            await DisplayAlertAsync(
                "Registration Error",
                "Unable to create your account. Please try again.",
                "OK");
        }
    }
    private async void BackClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

}
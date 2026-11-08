
using CustomGarmentApp.Services;
using System.Text.RegularExpressions;

namespace CustomGarmentApp.Pages;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
    }

    // Validates and updates the user's password.
    private async void ChangePasswordClicked(object? sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim() ?? "";
        string currentPassword = CurrentPasswordEntry.Text ?? "";
        string newPassword = NewPasswordEntry.Text ?? "";
        string confirmPassword = ConfirmPasswordEntry.Text ?? "";

        // Check for missing information.
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(currentPassword) ||
            string.IsNullOrWhiteSpace(newPassword) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please complete all fields.",
                "OK");
            return;
        }

        // Enforce password complexity requirements.
        bool validPassword =
            newPassword.Length >= 8 &&
            Regex.IsMatch(newPassword, @"[A-Z]") &&
            Regex.IsMatch(newPassword, @"[a-z]") &&
            Regex.IsMatch(newPassword, @"[0-9]") &&
            Regex.IsMatch(newPassword, @"[^a-zA-Z0-9]");

        if (!validPassword)
        {
            await DisplayAlertAsync(
                "Invalid Password",
                "Use at least 8 characters, including uppercase, " +
                "lowercase, a number, and a special character.",
                "OK");
            return;
        }

        // Confirm that the new passwords match.
        if (newPassword != confirmPassword)
        {
            await DisplayAlertAsync(
                "Password Mismatch",
                "New passwords do not match.",
                "OK");
            return;
        }

        try
        {
            UserDatabase database = new UserDatabase();

            // Verify the current password before updating.
            bool changed = await database.ChangePasswordAsync(
                email, currentPassword, newPassword);

            if (!changed)
            {
                await DisplayAlertAsync(
                    "Update Failed",
                    "Incorrect email or current password.",
                    "OK");
                return;
            }

            // Remove passwords from the input fields.
            CurrentPasswordEntry.Text = "";
            NewPasswordEntry.Text = "";
            ConfirmPasswordEntry.Text = "";

            await DisplayAlertAsync(
                "Success",
                "Your password has been updated.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception)
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to update your password.",
                "OK");
        }
    }
}

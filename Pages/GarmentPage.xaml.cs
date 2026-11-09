using CustomGarmentApp.Models;
using CustomGarmentApp.Services;

namespace CustomGarmentApp.Pages;

public partial class GarmentPage : ContentPage
{
    private readonly GarmentDatabase _database = new();
    private Garment? _editingGarment;

    public GarmentPage()
    {
        InitializeComponent();
    }

    // Load saved garments when the page opens.

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Do not load protected content without a valid session.
        if (!SessionService.IsAuthenticated)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

        await LoadGarmentsAsync();
    }


    // Retrieve and display garments using the current search.
    private async Task LoadGarmentsAsync()
    {
        try
        {
            var garments = await _database.SearchGarmentsAsync(
                SearchEntry.Text ?? "");

            GarmentList.Children.Clear();

            foreach (var garment in garments)
            {
                // Build a visual card for each database record.
                var card = new VerticalStackLayout
                {
                    BackgroundColor = Color.FromArgb("#222222"),
                    Padding = new Thickness(15),
                    Spacing = 10
                };

                card.Children.Add(new Label
                {
                    Text = garment.Name,
                    TextColor = Colors.White,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                });

                card.Children.Add(new Label
                {
                    Text = garment.Description,
                    TextColor = Color.FromArgb("#B0B0B0")
                });

                var editButton = new Button
                {
                    Text = "EDIT",
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black
                };

                // Load selected garment into the editing form.
                editButton.Clicked += (sender, e) =>
                {
                    _editingGarment = garment;
                    NameEntry.Text = garment.Name;
                    DescriptionEntry.Text = garment.Description;
                    SaveButton.Text = "UPDATE GARMENT";
                };

                var deleteButton = new Button
                {
                    Text = "DELETE",
                    BackgroundColor = Color.FromArgb("#444444"),
                    TextColor = Colors.White
                };

                // Confirm deletion before removing a garment.
                deleteButton.Clicked += async (sender, e) =>
                {
                    bool confirm = await DisplayAlertAsync(
                        "Delete Garment",
                        $"Delete {garment.Name}?",
                        "Yes",
                        "No");

                    if (confirm)
                    {
                        await _database.DeleteGarmentAsync(garment);

                        if (_editingGarment?.Id == garment.Id)
                            ResetForm();

                        await LoadGarmentsAsync();
                    }
                };


                var detailsButton = new Button
                {
                    Text = "VIEW DETAILS",
                    BackgroundColor = Color.FromArgb("#555555"),
                    TextColor = Colors.White
                };

                detailsButton.Clicked += async (sender, e) =>
                {
                    if (!SessionService.IsAuthenticated)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                        return;
                    }

                    await Shell.Current.GoToAsync(
                        $"GarmentSummaryPage?garmentId={garment.Id}");
                };

                card.Children.Add(detailsButton);


                card.Children.Add(editButton);
                card.Children.Add(deleteButton);

                GarmentList.Children.Add(card);
            }

            ResultsLabel.Text = garments.Count == 0
                ? "No matching garments found."
                : $"{garments.Count} garment(s) found.";
        }
        catch (Exception)
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to load garments.",
                "OK");
        }
    }

    // Add a new garment or update the selected garment.
    private async void SaveClicked(object? sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim() ?? "";
        string description = DescriptionEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlertAsync(
                "Missing Information",
                "Please enter a garment name.",
                "OK");
            return;
        }

        try
        {
            if (_editingGarment == null)
            {
                // CREATE: Insert a new record.
                await _database.AddGarmentAsync(new Garment
                {
                    Name = name,
                    Description = description
                });
            }
            else
            {
                // UPDATE: Modify the existing record.
                _editingGarment.Name = name;
                _editingGarment.Description = description;

                await _database.UpdateGarmentAsync(_editingGarment);
            }

            ResetForm();
            await LoadGarmentsAsync();
        }
        catch (Exception)
        {
            await DisplayAlertAsync(
                "Error",
                "Unable to save garment.",
                "OK");
        }
    }

    // Search the SQLite garment records.
    private async void SearchClicked(object? sender, EventArgs e)
    {
        await LoadGarmentsAsync();
    }

    // Clear search and restore all records.
    private async void ClearClicked(object? sender, EventArgs e)
    {
        SearchEntry.Text = "";
        await LoadGarmentsAsync();
    }

    // Cancel editing without changing the database.
    private void CancelClicked(object? sender, EventArgs e)
    {
        ResetForm();
    }

    // Restore the form to Add Garment mode.
    private void ResetForm()
    {
        _editingGarment = null;
        NameEntry.Text = "";
        DescriptionEntry.Text = "";
        SaveButton.Text = "ADD GARMENT";
    }



    private async void LogoutClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Log Out",
            "Are you sure you want to log out?",
            "Yes",
            "Cancel");

        if (!confirm)
            return;

        // End the authenticated session.
        SessionService.EndSession();

        // Clear unfinished edits and search text.
        ResetForm();
        SearchEntry.Text = "";

        // Return to the login screen.
        await Shell.Current.GoToAsync("//LoginPage");
    }


}


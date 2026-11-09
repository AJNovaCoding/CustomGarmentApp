using CustomGarmentApp.Services;

namespace CustomGarmentApp.Pages;

[QueryProperty(nameof(GarmentId), "garmentId")]
public partial class GarmentSummaryPage : ContentPage
{
    private readonly GarmentDatabase _database = new();

    private string _garmentId = "";

    public string GarmentId
    {
        get => _garmentId;
        set => _garmentId = value;
    }

    public GarmentSummaryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!SessionService.IsAuthenticated)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

        if (!int.TryParse(GarmentId, out int id))
        {
            await DisplayAlertAsync(
                "Invalid Garment",
                "No valid garment was selected.",
                "OK");
            return;
        }

        var garments = await _database.GetGarmentsAsync();

        var selectedGarment = garments
            .FirstOrDefault(g => g.Id == id);

        if (selectedGarment == null)
        {
            await DisplayAlertAsync(
                "Not Found",
                "This garment could not be found.",
                "OK");
            return;
        }

        GarmentNameLabel.Text = selectedGarment.Name;
        GarmentDescriptionLabel.Text = selectedGarment.Description;
    }

    private async void BackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}


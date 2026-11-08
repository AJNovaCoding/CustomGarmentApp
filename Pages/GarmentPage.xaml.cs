
using System.Text.RegularExpressions;

namespace CustomGarmentApp.Pages;

public partial class GarmentPage : ContentPage
{
    public GarmentPage()
    {
        InitializeComponent();
    }

    // Searches the catalog using text and wildcard patterns.
    private void SearchClicked(object? sender, EventArgs e)
    {
        string search = SearchEntry.Text?.Trim() ?? "";

        // An empty search displays every garment.
        if (string.IsNullOrWhiteSpace(search))
        {
            ShowAllGarments();
            return;
        }

        // Convert * into a regex wildcard.
        // Escape other characters to avoid unintended patterns.
        string pattern = "^" +
            Regex.Escape(search).Replace(@"\*", ".*") +
            "$";

        // Plain text searches match anywhere in the garment name.
        if (!search.Contains('*'))
        {
            pattern = Regex.Escape(search);
        }

        bool jacketMatch = Regex.IsMatch(
            "CUSTOM JACKET", pattern, RegexOptions.IgnoreCase);

        bool dressMatch = Regex.IsMatch(
            "CUSTOM DRESS", pattern, RegexOptions.IgnoreCase);

        bool skirtMatch = Regex.IsMatch(
            "CUSTOM SKIRT", pattern, RegexOptions.IgnoreCase);

        // Display only matching garment cards.
        JacketCard.IsVisible = jacketMatch;
        DressCard.IsVisible = dressMatch;
        SkirtCard.IsVisible = skirtMatch;

        int count = (jacketMatch ? 1 : 0) +
                    (dressMatch ? 1 : 0) +
                    (skirtMatch ? 1 : 0);

        ResultsLabel.Text = count == 0
            ? "No matching garments found."
            : $"{count} garment(s) found.";
    }

    // Clears the search and restores the full catalog.
    private void ClearClicked(object? sender, EventArgs e)
    {
        SearchEntry.Text = "";
        ShowAllGarments();
    }

    // Displays every garment in the catalog.
    private void ShowAllGarments()
    {
        JacketCard.IsVisible = true;
        DressCard.IsVisible = true;
        SkirtCard.IsVisible = true;

        ResultsLabel.Text = "Showing all 3 garments";
    }
}

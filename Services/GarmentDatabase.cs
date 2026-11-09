using CustomGarmentApp.Models;
using SQLite;

namespace CustomGarmentApp.Services;

// Handles garment storage and CRUD operations using SQLite.
public class GarmentDatabase
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized;

    public GarmentDatabase()
    {
        // Store garment data in the application's local directory.
        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory, "garments.db3");

        _database = new SQLiteAsyncConnection(dbPath);
    }

    // Create the database table and initial garment records.
    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        await _database.CreateTableAsync<Garment>();

        // Add starter garments only when the database is empty.
        int count = await _database.Table<Garment>().CountAsync();

        if (count == 0)
        {
            await _database.InsertAllAsync(new List<Garment>
            {
                new Garment
                {
                    Name = "CUSTOM JACKET",
                    Description = "Personalize a jacket with colors, fabrics, and accessories."
                },
                new Garment
                {
                    Name = "CUSTOM DRESS",
                    Description = "Create a personalized dress with unique design options."
                },
                new Garment
                {
                    Name = "CUSTOM SKIRT",
                    Description = "Choose your preferred style, fabric, and embellishments."
                }
            });
        }

        _initialized = true;
    }

    // READ: Retrieve all garments from SQLite.
    public async Task<List<Garment>> GetGarmentsAsync()
    {
        await InitializeAsync();

        return await _database.Table<Garment>()
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    // CREATE: Add a new garment.
    public async Task<int> AddGarmentAsync(Garment garment)
    {
        await InitializeAsync();
        return await _database.InsertAsync(garment);
    }

    // UPDATE: Save changes to an existing garment.
    public async Task<int> UpdateGarmentAsync(Garment garment)
    {
        await InitializeAsync();
        return await _database.UpdateAsync(garment);
    }

    // DELETE: Remove a garment by its unique ID.
    public async Task<int> DeleteGarmentAsync(Garment garment)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(garment);
    }

    // SEARCH: Supports partial text and * wildcards.
    public async Task<List<Garment>> SearchGarmentsAsync(string search)
    {
        List<Garment> garments = await GetGarmentsAsync();

        if (string.IsNullOrWhiteSpace(search))
            return garments;

        search = search.Trim();

        // Escape special regex characters except the wildcard.
        string pattern = "^" +
            System.Text.RegularExpressions.Regex.Escape(search)
                .Replace(@"\*", ".*") + "$";

        // Without wildcards, match text anywhere in the name.
        if (!search.Contains('*'))
        {
            pattern = System.Text.RegularExpressions.Regex.Escape(search);
        }

        return garments.Where(g =>
            System.Text.RegularExpressions.Regex.IsMatch(
                g.Name,
                pattern,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            )).ToList();
    }
}


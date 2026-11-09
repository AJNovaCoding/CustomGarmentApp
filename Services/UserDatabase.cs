using System;
using System.Collections.Generic;
using System.Text;
using CustomGarmentApp.Models;
using SQLite;
using System.Security.Cryptography;

namespace CustomGarmentApp.Services
{
    public class UserDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public UserDatabase()
        {
            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory, "users.db3");

            _database = new SQLiteAsyncConnection(dbPath);
        }

        // Creates the user table if it does not already exist.

        public async Task InitializeAsync()
        {
            // Creates the Users table if it does not already exist.
            // including IsAdmin, to an existing table.
            await _database.CreateTableAsync<User>();
        }


        // Converts a password into a salted, one-way PBKDF2 hash.
        private static string HashPassword(
            string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                210000,
                HashAlgorithmName.SHA256,
                32);

            return Convert.ToBase64String(hash);
        }

        // Registers a new account and rejects duplicate email addresses.
        public async Task<bool> RegisterAsync(
            string fullName, string email, string password)
        {
            await InitializeAsync();

            email = email.Trim().ToLowerInvariant();

            User? existing = await _database.Table<User>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (existing != null)
                return false;

            byte[] salt = RandomNumberGenerator.GetBytes(16);

            User user = new()
            {
                FullName = fullName.Trim(),
                Email = email,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = HashPassword(password, salt)
            };

            await _database.InsertAsync(user);
            return true;
        }

        // Checks whether the supplied credentials match a saved account.
        public async Task<bool> LoginAsync(
            string email, string password)
        {
            await InitializeAsync();

            email = email.Trim().ToLowerInvariant();

            User? user = await _database.Table<User>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            byte[] salt = Convert.FromBase64String(
                user.PasswordSalt);

            byte[] expectedHash = Convert.FromBase64String(
                user.PasswordHash);

            byte[] actualHash = Convert.FromBase64String(
                HashPassword(password, salt));

            return CryptographicOperations.FixedTimeEquals(
                expectedHash, actualHash);
        }

        // Changes a password after verifying the existing password.
        public async Task<bool> ChangePasswordAsync(
            string email, string oldPassword, string newPassword)
        {
            if (!await LoginAsync(email, oldPassword))
                return false;

            User? user = await _database.Table<User>()
                .Where(u => u.Email == email.Trim().ToLowerInvariant())
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            byte[] newSalt = RandomNumberGenerator.GetBytes(16);

            user.PasswordSalt = Convert.ToBase64String(newSalt);
            user.PasswordHash = HashPassword(newPassword, newSalt);

            await _database.UpdateAsync(user);
            return true;
        }

        // Checks whether an existing user has administrator privileges.
        public async Task<bool> IsAdministratorAsync(string email)
        {
            await InitializeAsync();

            string normalizedEmail = email.Trim().ToLowerInvariant();

            var user = await _database.Table<User>()
                .Where(u => u.Email == normalizedEmail)
                .FirstOrDefaultAsync();

            return user?.IsAdmin == true;
        }

    }
}


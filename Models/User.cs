using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CustomGarmentApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        [Unique]
        public string Email { get; set; } = "";

        // Store password hashes and salts, never plaintext passwords.
        public string PasswordHash { get; set; } = "";

        public string PasswordSalt { get; set; } = "";

        public bool IsAdmin { get; set; } = false;
    }
}

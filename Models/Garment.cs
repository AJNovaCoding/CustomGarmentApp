using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomGarmentApp.Models
{
    public class Garment
    {
        // Unique identifier assigned automatically by SQLite.
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Name of the garment.
        [NotNull]
        public string Name { get; set; } = "";

        // Description of the garment.
        public string Description { get; set; } = "";
    }
}

using SQLite;

namespace frogaDB8.Models
{
    /// <summary>
    /// Bazkideak klasea datu baseko "Bazkideak" taula ordezkatzen du.
    /// </summary>
    public class Bazkideak
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Izena { get; set; }
        public string? Helbidea { get; set; }
        public int? Telefono { get; set; }
        public int? PK { get; set; }
        public string? Herria { get; set; }
    }
}

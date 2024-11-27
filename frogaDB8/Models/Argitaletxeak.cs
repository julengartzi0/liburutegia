using SQLite;

namespace frogaDB8.Models
{
    /// <summary>
    /// Argitaletxeak klasea datu baseko "Argitaletxeak" taula ordezkatzen du.
    /// </summary>
    public class Argitaletxeak
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Izena { get; set; }
    }
}

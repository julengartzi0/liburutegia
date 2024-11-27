using SQLite;

namespace frogaDB8.Models
{
    /// <summary>
    /// Egileak klasea datu baseko "Egileak" taula ordezkatzen du.
    /// Egile bakoitzaren identifikazioa eta izena gordetzen ditu.
    /// </summary>
    public class Egileak
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Izena { get; set; } 
    }
}

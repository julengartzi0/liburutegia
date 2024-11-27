using SQLite;

namespace frogaDB8.Models
{
    /// <summary>
    /// Hizkuntzak klasea datu baseko "Hizkuntzak" taula ordezkatzen du.
    /// Taula honek hizkuntza bakoitzaren identifikazioa eta izena gordetzen ditu.
    /// </summary>
    public class Hizkuntzak
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Nombre { get; set; }
    }
}

using SQLite;

namespace frogaDB8.Models
{
    /// <summary>
    /// Maileguak klasea datu baseko "Maileguak" taula ordezkatzen du.
    /// Taula honek Mailegu bakoitzaren informazioa gordetzen du.
    /// </summary
    public class Maileguak
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int? IdBazkidea { get; set; } // Bazkidearen ID-a (Bazkideak taularekin erlazioa).

        public int? IdLiburua { get; set; } // Liburuaren ID-a (Liburuak taularekin erlazioa).

        public string? MaileguData { get; set; } // Maileguaren data.

        public int? Itzulita { get; set; } // Liburua itzuli den adierazlea (0 edo 1).
    }
}

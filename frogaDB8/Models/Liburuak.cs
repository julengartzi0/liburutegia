using SQLite;

namespace frogaDB8.Models
{
    /// <summary>
    /// Liburuak klasea datu baseko "Liburuak" taula ordezkatzen du.
    /// Liburu bakoitzaren informazioa gordetzen du, besteak beste, ISBN zenbakia, izenburua, argitaletxea, egilea, sinopsia, kopien kopurua eta hizkuntza.
    /// </summary>
    public class Liburuak
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int? ISBN { get; set; } 

        public string? Izenburua { get; set; } 

        public int? IdArgitaletxea { get; set; } 

        public int? IdEgilea { get; set; } 

        public string? Sinopsia { get; set; } 

        public int? Kopiak { get; set; } 

        public int? IdIHizkuntza { get; set; } 
    }
}

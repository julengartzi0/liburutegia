using System.Collections.Generic;
using System.Threading.Tasks;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// LiburuakService klasea datu baseko "Liburuak" taularen kudeaketa egiteko erabiltzen da.
    /// Liburuen informazioa lortu, gehitu, eguneratu eta ezabatzeko metodoak eskaintzen ditu.
    /// </summary>
    public class LiburuakService
    {
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Eraikitzailea: SQLiteAsyncConnection objektua ezartzen du.
        /// </summary>
        /// <param name="database">Datu basearekin konektatzeko objektua.</param>
        public LiburuakService(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Liburu guztiak lortzen ditu "Liburuak" taulatik.
        /// </summary>
        /// <returns>Liburuen zerrenda asinkronikoki itzultzen du.</returns>
        public Task<List<Liburuak>> EskuratuLiburuakAsync()
        {
            return _database.Table<Liburuak>().ToListAsync();
        }

        /// <summary>
        /// Liburu berria gehitzen du "Liburuak" taulan.
        /// </summary>
        /// <param name="liburua">Gehitu nahi den liburuaren objektua.</param>
        /// <returns>Txertaketaren emaitza itzultzen du.</returns>
        public Task<int> GehituLiburuakAsync(Liburuak liburua)
        {
            return _database.InsertAsync(liburua);
        }

        /// <summary>
        /// Liburuaren datuak eguneratzen ditu "Liburuak" taulan.
        /// </summary>
        /// <param name="liburua">Eguneratu nahi den liburuaren objektua.</param>
        /// <returns>Eguneratzearen emaitza itzultzen du.</returns>
        public Task<int> AktualizatuLiburuakAsync(Liburuak liburua)
        {
            return _database.UpdateAsync(liburua);
        }

        /// <summary>
        /// Liburu bat ezabatzen du "Liburuak" taulatik.
        /// </summary>
        /// <param name="liburua">Ezabatu nahi den liburuaren objektua.</param>
        /// <returns>Ezabaketaren emaitza itzultzen du.</returns>
        public Task<int> EzabatuLiburuakAsync(Liburuak liburua)
        {
            return _database.DeleteAsync(liburua);
        }

        /// <summary>
        /// Bazkide batek maileguan dituen liburuak lortzen ditu, itzulita ez daudenak bakarrik.
        /// </summary>
        /// <param name="bazkideaId">Bazkidearen ID-a.</param>
        /// <returns>Bazkideak maileguan dituen liburuen zerrenda itzultzen du.</returns>
        public async Task<List<Liburuak>> EskuratuLibrosPrestadosPorBazkideaAsync(int bazkideaId)
        {
            var query = @"SELECT Liburuak.Id AS Id, Liburuak.ISBN, Liburuak.Izenburua, 
                         Liburuak.IdArgitaletxea, Liburuak.IdEgilea, Liburuak.Sinopsia, 
                         Liburuak.Kopiak, Liburuak.IdIHizkuntza 
                  FROM Liburuak
                  INNER JOIN Maileguak ON Liburuak.Id = Maileguak.IdLiburua
                  WHERE Maileguak.IdBazkidea = ? AND Maileguak.Itzulita = 0";
            return await _database.QueryAsync<Liburuak>(query, bazkideaId);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// EgileakService klasea datu baseko "Egileak" taularen kudeaketa egiteko erabiltzen da.
    /// Egileen informazioa lortu, gehitu, eguneratu eta ezabatzeko metodoak eskaintzen ditu.
    /// </summary>
    public class EgileakService
    {
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Eraikitzailea: SQLiteAsyncConnection objektua ezartzen du.
        /// </summary>
        /// <param name="database">Datu basearekin konektatzeko objektua.</param>
        public EgileakService(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Egile guztiak lortzen ditu "Egileak" taulatik.
        /// </summary>
        /// <returns>Egileen zerrenda asinkronikoki itzultzen du.</returns>
        public Task<List<Egileak>> EskuratuEgileakAsync()
        {
            return _database.Table<Egileak>().ToListAsync();
        }

        /// <summary>
        /// Egile berria gehitzen du "Egileak" taulan.
        /// </summary>
        /// <param name="egile">Gehitu nahi den egilearen objektua.</param>
        /// <returns>Txertaketaren emaitza itzultzen du.</returns>
        public Task<int> GehituEgileakAsync(Egileak egile)
        {
            return _database.InsertAsync(egile);
        }

        /// <summary>
        /// Egilearen datuak eguneratzen ditu "Egileak" taulan.
        /// </summary>
        /// <param name="egile">Eguneratu nahi den egilearen objektua.</param>
        /// <returns>Eguneratzearen emaitza itzultzen du.</returns>
        public Task<int> AktualizatuEgileakAsync(Egileak egile)
        {
            return _database.UpdateAsync(egile);
        }

        /// <summary>
        /// Egile bat ezabatzen du "Egileak" taulatik.
        /// </summary>
        /// <param name="egile">Ezabatu nahi den egilearen objektua.</param>
        /// <returns>Ezabaketaren emaitza itzultzen du.</returns>
        public Task<int> EzabatuEgileakAsync(Egileak egile)
        {
            return _database.DeleteAsync(egile);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// BazkideakService klasea datu baseko "Bazkideak" taularen kudeaketa egiteko erabiltzen da.
    /// Bazkideen informazioa lortu, gehitu, eguneratu eta ezabatzeko metodoak eskaintzen ditu.
    /// </summary>
    public class BazkideakService
    {
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Eraikitzailea: SQLiteAsyncConnection objektua ezartzen du.
        /// </summary>
        /// <param name="database">Datu basearekin konektatzeko objektua.</param>
        public BazkideakService(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Bazkide guztiak lortzen ditu "Bazkideak" taulatik.
        /// </summary>
        /// <returns>Bazkideen zerrenda asinkronikoki itzultzen du.</returns>
        public Task<List<Bazkideak>> EskuratuBazkideakAsync()
        {
            return _database.Table<Bazkideak>().ToListAsync();
        }

        /// <summary>
        /// Bazkide berria gehitzen du "Bazkideak" taulan.
        /// </summary>
        /// <param name="bazkide">Gehitu nahi den bazkidearen objektua.</param>
        /// <returns>Txertaketaren emaitza itzultzen du.</returns>
        public Task<int> GehituBazkideakAsync(Bazkideak bazkide)
        {
            return _database.InsertAsync(bazkide);
        }

        /// <summary>
        /// Bazkidearen datuak eguneratzen ditu "Bazkideak" taulan.
        /// </summary>
        /// <param name="bazkide">Eguneratu nahi den bazkidearen objektua.</param>
        /// <returns>Eguneratzearen emaitza itzultzen du.</returns>
        public Task<int> AktualizatuBazkideakAsync(Bazkideak bazkide)
        {
            return _database.UpdateAsync(bazkide);
        }

        /// <summary>
        /// Bazkide bat ezabatzen du "Bazkideak" taulatik.
        /// </summary>
        /// <param name="bazkide">Ezabatu nahi den bazkidearen objektua.</param>
        /// <returns>Ezabaketaren emaitza itzultzen du.</returns>
        public Task<int> EzabatuBazkideakAsync(Bazkideak bazkide)
        {
            return _database.DeleteAsync(bazkide);
        }
    }
}

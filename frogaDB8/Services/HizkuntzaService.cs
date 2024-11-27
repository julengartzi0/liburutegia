using System.Collections.Generic;
using System.Threading.Tasks;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// HizkuntzakService klasea datu baseko "Hizkuntzak" taularen kudeaketa egiteko erabiltzen da.
    /// Hizkuntzen informazioa lortu, gehitu, eguneratu eta ezabatzeko metodoak eskaintzen ditu.
    /// </summary>
    public class HizkuntzakService
    {
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Eraikitzailea: SQLiteAsyncConnection objektua ezartzen du.
        /// </summary>
        /// <param name="database">Datu basearekin konektatzeko objektua.</param>
        public HizkuntzakService(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Hizkuntza guztiak lortzen ditu "Hizkuntzak" taulatik.
        /// </summary>
        /// <returns>Hizkuntzen zerrenda asinkronikoki itzultzen du.</returns>
        public Task<List<Hizkuntzak>> EskuratuHizkuntzakAsync()
        {
            return _database.Table<Hizkuntzak>().ToListAsync();
        }

        /// <summary>
        /// Hizkuntza berria gehitzen du "Hizkuntzak" taulan.
        /// </summary>
        /// <param name="hizkuntza">Gehitu nahi den hizkuntzaren objektua.</param>
        /// <returns>Txertaketaren emaitza itzultzen du.</returns>
        public Task<int> GehituHizkuntzakAsync(Hizkuntzak hizkuntza)
        {
            return _database.InsertAsync(hizkuntza);
        }

        /// <summary>
        /// Hizkuntzaren datuak eguneratzen ditu "Hizkuntzak" taulan.
        /// </summary>
        /// <param name="hizkuntza">Eguneratu nahi den hizkuntzaren objektua.</param>
        /// <returns>Eguneratzearen emaitza itzultzen du.</returns>
        public Task<int> AktualizatuHizkuntzakAsync(Hizkuntzak hizkuntza)
        {
            return _database.UpdateAsync(hizkuntza);
        }

        /// <summary>
        /// Hizkuntza bat ezabatzen du "Hizkuntzak" taulatik.
        /// </summary>
        /// <param name="hizkuntza">Ezabatu nahi den hizkuntzaren objektua.</param>
        /// <returns>Ezabaketaren emaitza itzultzen du.</returns>
        public Task<int> EzabatuHizkuntzakAsync(Hizkuntzak hizkuntza)
        {
            return _database.DeleteAsync(hizkuntza);
        }
    }
}

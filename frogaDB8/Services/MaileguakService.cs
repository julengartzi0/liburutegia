using System.Collections.Generic;
using System.Threading.Tasks;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// MaileguakService klasea datu baseko "Maileguak" taularen kudeaketa egiteko erabiltzen da.
    /// Maileguen informazioa lortu, gehitu, eguneratu eta ezabatzeko metodoak eskaintzen ditu.
    /// </summary>
    public class MaileguakService
    {
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Eraikitzailea: SQLiteAsyncConnection objektua ezartzen du.
        /// </summary>
        /// <param name="database">Datu basearekin konektatzeko objektua.</param>
        public MaileguakService(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Mailegu guztiak lortzen ditu "Maileguak" taulatik.
        /// </summary>
        /// <returns>Maileguen zerrenda asinkronikoki itzultzen du.</returns>
        public Task<List<Maileguak>> EskuratuMaileguakAsync()
        {
            return _database.Table<Maileguak>().ToListAsync();
        }

        /// <summary>
        /// Mailegu berria gehitzen du "Maileguak" taulan.
        /// </summary>
        /// <param name="mailegu">Gehitu nahi den maileguaren objektua.</param>
        /// <returns>Txertaketaren emaitza itzultzen du.</returns>
        public Task<int> GehituMaileguakAsync(Maileguak mailegu)
        {
            return _database.InsertAsync(mailegu);
        }

        /// <summary>
        /// Maileguaren datuak eguneratzen ditu "Maileguak" taulan.
        /// </summary>
        /// <param name="mailegu">Eguneratu nahi den maileguaren objektua.</param>
        /// <returns>Eguneratzearen emaitza itzultzen du.</returns>
        public Task<int> AktualizatuMaileguakAsync(Maileguak mailegu)
        {
            return _database.UpdateAsync(mailegu);
        }

        /// <summary>
        /// Mailegu bat ezabatzen du "Maileguak" taulatik.
        /// </summary>
        /// <param name="mailegu">Ezabatu nahi den maileguaren objektua.</param>
        /// <returns>Ezabaketaren emaitza itzultzen du.</returns>
        public Task<int> EzabatuMaileguakAsync(Maileguak mailegu)
        {
            return _database.DeleteAsync(mailegu);
        }
    }
}

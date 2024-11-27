using System.Collections.Generic;
using System.Threading.Tasks;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// ArgitaletxeakService klasea datu baseko "Argitaletxeak" taularen kudeaketa egiteko erabiltzen da.
    /// </summary>
    public class ArgitaletxeakService
    {
        private readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Eraikitzailea: SQLiteAsyncConnection objektua ezartzen du.
        /// </summary>
        /// <param name="database">Datu basearekin konektatzeko objektua.</param>
        public ArgitaletxeakService(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Argitaletxe guztiak lortzen ditu "Argitaletxeak" taulatik.
        /// </summary>
        /// <returns>Argitaletxeen zerrenda asinkronikoki itzultzen du.</returns>
        public Task<List<Argitaletxeak>> EskuratuArgitaletxeakAsync()
        {
            return _database.Table<Argitaletxeak>().ToListAsync();
        }

        /// <summary>
        /// Argitaletxe berria gehitzen du "Argitaletxeak" taulan.
        /// </summary>
        /// <param name="argitaletxe">Gehitu nahi den argitaletxearen objektua.</param>
        /// <returns>Txertaketaren emaitza itzultzen du.</returns>
        public Task<int> GehituArgitaletxeakAsync(Argitaletxeak argitaletxe)
        {
            return _database.InsertAsync(argitaletxe);
        }

        /// <summary>
        /// Argitaletxearen datuak eguneratzen ditu "Argitaletxeak" taulan.
        /// </summary>
        /// <param name="argitaletxe">Eguneratu nahi den argitaletxearen objektua.</param>
        /// <returns>Eguneratzearen emaitza itzultzen du.</returns>
        public Task<int> AktualizatuArgitaletxeakAsync(Argitaletxeak argitaletxe)
        {
            return _database.UpdateAsync(argitaletxe);
        }

        /// <summary>
        /// Argitaletxe bat ezabatzen du "Argitaletxeak" taulatik.
        /// </summary>
        /// <param name="argitaletxe">Ezabatu nahi den argitaletxearen objektua.</param>
        /// <returns>Ezabaketaren emaitza itzultzen du.</returns>
        public Task<int> EzabatuArgitaletxeakAsync(Argitaletxeak argitaletxe)
        {
            return _database.DeleteAsync(argitaletxe);
        }
    }
}

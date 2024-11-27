using SQLite;

namespace frogaDB8.Services
{
    /// <summary>
    /// DatabaseService klasea datu basearen kudeaketa zentralizatzeko erabiltzen da.
    /// Klase honek datu baseko taula bakoitzaren zerbitzu espezifikoak eskaintzen ditu,
    /// eta konexio bakarra erabiltzen du guztientzat.
    /// </summary>
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        // Hizkuntzak taularen kudeaketa egiteko zerbitzua
        public HizkuntzakService HizkuntzakService { get; }

        // Argitaletxeak taularen kudeaketa egiteko zerbitzua
        public ArgitaletxeakService ArgitaletxeakService { get; }

        // Bazkideak taularen kudeaketa egiteko zerbitzua
        public BazkideakService BazkideakService { get; }

        // Egileak taularen kudeaketa egiteko zerbitzua
        public EgileakService EgileakService { get; }

        // Liburuak taularen kudeaketa egiteko zerbitzua
        public LiburuakService LiburuakService { get; }

        // Maileguak taularen kudeaketa egiteko zerbitzua
        public MaileguakService MaileguakService { get; }

        /// <summary>
        /// Eraikitzailea: datu basearen konexioa sortzen du eta zerbitzuak inicializatzen ditu.
        /// </summary>
        public DatabaseService()
        {
            _database = new SQLiteAsyncConnection(DatabaseInitializer.DbPath);

            // Zerbitzu espezifikoak inicializatu konexio partekatuarekin
            HizkuntzakService = new HizkuntzakService(_database);
            ArgitaletxeakService = new ArgitaletxeakService(_database);
            BazkideakService = new BazkideakService(_database);
            EgileakService = new EgileakService(_database);
            LiburuakService = new LiburuakService(_database);
            MaileguakService = new MaileguakService(_database);
        }
    }
}

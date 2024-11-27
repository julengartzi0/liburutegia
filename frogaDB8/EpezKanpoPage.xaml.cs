using frogaDB8.ViewModels;
using frogaDB8.Services;

namespace frogaDB8
{
    /// <summary>
    /// Epez Kanpo dauden maileguen orrialdea.
    /// </summary>
    public partial class EpezKanpoPage : ContentPage
    {
        /// <summary>
        /// EpezKanpoPage klasearen instantzia sortzen du.
        /// </summary>
        /// <param name="databaseService">Datubasearekin elkarreragiteko zerbitzua.</param>
        public EpezKanpoPage(DatabaseService databaseService)
        {
            InitializeComponent();

            // ViewModela sortzen du eta datu-iturriak injektatzen dizkio
            BindingContext = new EpezKanpoViewModel(
                databaseService.MaileguakService, // Maileguen zerbitzua
                databaseService.LiburuakService, // Liburuen zerbitzua
                databaseService.BazkideakService // Bazkideen zerbitzua
            );
        }
    }
}

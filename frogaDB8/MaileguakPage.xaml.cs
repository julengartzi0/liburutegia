using frogaDB8.Services;
using frogaDB8.ViewModels;
using Microsoft.Maui.Controls;

namespace frogaDB8
{
    /// <summary>
    /// MaileguakPage klasea: Maileguen kudeaketa orrialdea.
    /// </summary>
    public partial class MaileguakPage : ContentPage
    {
        /// <summary>
        /// MaileguakPage klasearen instantzia sortzen du.
        /// </summary>
        /// <param name="databaseService">Datubasearekin elkarreragiteko zerbitzua.</param>
        public MaileguakPage(DatabaseService databaseService)
        {
            InitializeComponent();

            // Orrialdearen BindingContext-a konfiguratu ViewModelarekin
            BindingContext = new MaileguakViewModel(
                databaseService.LiburuakService, // Liburuen zerbitzua
                databaseService.BazkideakService, // Bazkideen zerbitzua
                databaseService.MaileguakService // Maileguen zerbitzua
            );
        }
    }
}

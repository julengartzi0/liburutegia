using frogaDB8.Services;
using frogaDB8.ViewModels;
using Microsoft.Maui.Controls;

namespace frogaDB8
{
    /// <summary>
    /// LiburuakPage klasea: Liburuen informazioa kudeatzeko orrialdea.
    /// </summary>
    public partial class LiburuakPage : ContentPage
    {
        private DatabaseService _databaseService;

        /// <summary>
        /// LiburuakPage klasearen instantzia sortzen du.
        /// </summary>
        /// <param name="databaseService">Datubasearekin elkarreragiteko zerbitzua.</param>
        public LiburuakPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;

            // ViewModela sortu eta datu-iturriak pasa
            BindingContext = new LiburuakViewModel(
                _databaseService.LiburuakService, // Liburuen zerbitzua
                _databaseService.EgileakService,  // Egileen zerbitzua
                _databaseService.ArgitaletxeakService, // Argitaletxeen zerbitzua
                _databaseService.HizkuntzakService // Hizkuntzen zerbitzua
            );

            // Datuak gordetzeko mezuaren suskripzioa
            MessagingCenter.Subscribe<LiburuakViewModel>(this, "DatuakGordeta", async (sender) =>
            {
                await DisplayAlert("Baieztatu", "Datuak ondo gorde dira", "OK");
            });

            // Liburua gordetzeko beharrezko mezuaren suskripzioa
            MessagingCenter.Subscribe<LiburuakViewModel>(this, "LiburuaEzDaGorde", async (sender) =>
            {
                await DisplayAlert("Kontuz", "Liburua gorde behar da berri bat sartu aurretik.", "OK");
            });
        }

        /// <summary>
        /// Orrialdea desagertzean, mezularitzaren suskripzioak bertan behera uzten ditu.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LiburuakViewModel>(this, "DatuakGordeta");
            MessagingCenter.Unsubscribe<LiburuakViewModel>(this, "LiburuaEzDaGorde");
        }

        /// <summary>
        /// Testu-sarreraren aldaketa kudeatzen du, ziurtatuz bakarrik zenbakizko balioak onartzen direla.
        /// </summary>
        /// <param name="sender">Testu-sarrera kontrola.</param>
        /// <param name="e">Testuaren aldaketa gertakariaren datuak.</param>
        private void OnNumericEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (!string.IsNullOrWhiteSpace(e.NewTextValue) && !int.TryParse(e.NewTextValue, out _))
            {
                // Testua aurreko baliora itzultzen da balio berria ez bada zenbakizkoa
                entry.Text = e.OldTextValue;
            }
        }
    }
}

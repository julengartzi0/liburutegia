using frogaDB8.Services;
using Microsoft.Maui.Controls;

namespace frogaDB8
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService(); 
            DatabaseInitializer.InitializeDatabase(); 
        }

        private async void OnLiburuakClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LiburuakPage(_databaseService));
        }

        private async void OnMaileguaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MaileguakPage(_databaseService));
        }

        private async void OnEpezKanpoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EpezKanpoPage(_databaseService));
        }

        private void OnAteraClicked(object sender, EventArgs e)
        {
            Application.Current?.Quit();
        }
    }
}

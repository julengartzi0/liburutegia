using frogaDB8.Services;

namespace frogaDB8;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        DatabaseInitializer.InitializeDatabase();
        MainPage = new AppShell();
    }
}
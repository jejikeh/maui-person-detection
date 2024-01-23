using PersonDetection.Client.Gui.Views.Pages;

namespace PersonDetection.Client;

public partial class App
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new EntryPage());
    }
}
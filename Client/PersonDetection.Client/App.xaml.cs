using PersonDetection.Client.ViewModels;

namespace PersonDetection.Client;

public partial class App
{
    
    public App()
    {
        InitializeComponent();
        
        MainPage = new AppShell();
    }
}
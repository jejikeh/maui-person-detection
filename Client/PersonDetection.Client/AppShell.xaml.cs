using PersonDetection.Client.Configuration;

namespace PersonDetection.Client;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        MauiAppConfiguration.RegisterRoutes();
    }
}
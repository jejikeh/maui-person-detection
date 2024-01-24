using Microsoft.Maui.Controls;
using PersonDetection.Client.Pages;

namespace PersonDetection.Client;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(ChoosePhotoPage), typeof(ChoosePhotoPage));
    }
}
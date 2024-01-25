using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;

namespace PersonDetection.Client.Models;

public partial class ViewPhotoPair : ObservableObject
{
    [ObservableProperty]
    private ImageSource _original = ImageSource.FromFile("dotnet_bot.png");
    
    [ObservableProperty]
    private ImageSource _processed = ImageSource.FromFile("dotnet_bot.png");
    
    [ObservableProperty]
    private int _id;
}
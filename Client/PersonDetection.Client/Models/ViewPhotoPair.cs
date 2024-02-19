using CommunityToolkit.Mvvm.ComponentModel;

namespace PersonDetection.Client.Models;

public partial class ViewPhotoPair : ObservableObject
{
    // To initialize ImageSource from the beginning. On Android, without this, some images may not be displayed properly.    [ObservableProperty]
    [ObservableProperty]
    private ImageSource _original = ImageSource.FromFile("dotnet_bot.png");
    
    [ObservableProperty]
    private ImageSource _processed = ImageSource.FromFile("dotnet_bot.png");
    
    [ObservableProperty]
    private int _id;
}
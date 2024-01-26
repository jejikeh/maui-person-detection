using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Services;
using PersonDetection.Client.Models;

namespace PersonDetection.Client.ViewModels;

[QueryProperty(nameof(ViewPhotoPair), "ViewPhotoPair")]
public partial class PhotoViewModel(
    IPhotoGallery photoGallery, 
    PhotoSaverService photoSaverService) : ObservableObject
{
    [ObservableProperty]
    private ViewPhotoPair _viewPhotoPair = default!;
    
    [RelayCommand]
    private async Task Back() => await Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task Delete()
    {
        var pair = await photoGallery.GetPhotosByIdAsync(ViewPhotoPair.Id);
        if (pair is null)
        {
            await Shell.Current.DisplayAlert("Error", "Photo not found", "Ok");
            await Back();
        }
        
        await photoGallery.DeletePairAsync(pair!.Value.Original);
        await Back();
    }

    [RelayCommand]
    private async Task Save()
    {
        var photos = await photoGallery.GetPhotosByIdAsync(ViewPhotoPair.Id);
        if (photos is null)
        {
            await Shell.Current.DisplayAlert("Error", "Photo not found", "Ok");
            return;
        }
        
        await photoSaverService.UserSavePhotoAsync(photos.Value.Processed);
    }
}
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Extensions;
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
        var getPhotosResult = await photoGallery.GetPhotosByIdAsync(ViewPhotoPair.Id);
        if (getPhotosResult.IsError)
        {
            await getPhotosResult.GetError().DisplayErrorAsync();
            await Back();
        }

        var photo = getPhotosResult.GetValue();
        var deletePairResult = await photoGallery.DeletePairAsync(photo.Original);
        if (deletePairResult.IsError)
        {
            await deletePairResult.GetError().DisplayErrorAsync();
        }
        else
        {
            await Toast.Make($"{deletePairResult.GetValue()}").Show();
        }
        
        await Back();
    }

    [RelayCommand]
    private async Task Save()
    {
        var result = await photoGallery.GetPhotosByIdAsync(ViewPhotoPair.Id);
        if (result.IsError)
        {
            await result.GetError().DisplayErrorAsync();
            return;
        }
        
        await photoSaverService.UserSavePhotoAsync(result.GetValue().Processed);
    }
}
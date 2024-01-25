using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Models;
using PersonDetection.Client.Pages;
using PersonDetection.Client.Services;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace PersonDetection.Client.ViewModels;

public partial class ChoosePhotoViewModel(
    PhotoService photoService, 
    IPhotoGallery photoGallery,
    IPlatformImageSourceLoader imageSourceLoader) : ObservableObject
{
    [ObservableProperty]
    private ObservableRangeCollection<ViewPhotoPair> _photos = new();
    
    [ObservableProperty]
    private int _imageHeight = (int)Shell.Current.Height / 3;
    
    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task AddNewPhoto()
    {
        var photo = await photoService.NewPhoto();
        if (photo is null)
        {
            await Shell.Current.DisplayAlert("Error", "Something went wrong", "Ok");
            return;
        }

        Photos.Add(imageSourceLoader.LoadViewPhotoPair(
            photo.Value.Original,
            photo.Value.Processed
        ));
    }

    [RelayCommand]
    private async Task SelectPhotoCommand(int id)
    {
        var photos = await photoGallery.GetPhotosByIdAsync(id);
        if (photos is null)
        {
            await Shell.Current.DisplayAlert("Error", "Photo not found", "Ok");
            return;
        }

        await Shell.Current.GoToAsync(nameof(PhotoPage), true, new Dictionary<string, object>()
        {
            {
                "ViewPhotoPair", imageSourceLoader.LoadViewPhotoPair(photos.Value.Original, photos.Value.Processed)
            }
        });
    }
    
    public async Task LoadPhotosAsync()
    {
        Photos.Clear();
        var photoPairs = await photoGallery.GetPhotoPairsAsync();
        foreach (var photoPair in photoPairs)
        {
            var photos = await photoGallery.GetPhotosAsync(photoPair);
            if (photos is null)
            {
                continue;
            }

            var viewPhoto = imageSourceLoader.LoadViewPhotoPair(
                photos.Value.Original,
                photos.Value.Processed);
            
            Photos.Add(viewPhoto);
        }
    }
}
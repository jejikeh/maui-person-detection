using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Application.Services.Implementations;
using PersonDetection.Client.Extensions;
using PersonDetection.Client.Models;
using PersonDetection.Client.Pages;
using PersonDetection.Client.Services;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace PersonDetection.Client.ViewModels;

public partial class ChoosePhotoViewModel(
    PhotoService _photoService, 
    IPhotoGallery _photoGallery,
    IPlatformImageSourceLoader _imageSourceLoader) : ObservableObject
{
    [ObservableProperty]
    private ObservableRangeCollection<ViewPhotoPair> _photos = new();
    
    [ObservableProperty]
    private int _imageHeight = (int)Shell.Current.Height / 3;

    [ObservableProperty] 
    private bool _canAddPhoto = true;

    [RelayCommand]
    private async Task AddNewPhoto()
    {
        if (!CanAddPhoto)
        {
            await Toast.Make("Please wait until the photo is processed").Show();
            
            return;
        }
        
        CanAddPhoto = false;
        
        var result = await Task.Run(async () => await _photoService.NewPhotoToGalleryAsync());
        CanAddPhoto = true;

        if (result.IsError)
        {
            await result.GetError().ToastErrorAsync();
            
            return;
        }
        
        var photo = result.GetValue();
        Photos.Add(_imageSourceLoader.LoadViewPhotoPair(
            photo.Original,
            photo.Processed
        ));
    }
    
    [RelayCommand]
    private async Task StreamCamera()
    {
        await Shell.Current.GoToAsync(nameof(StreamCameraPage), true);
    }

    [RelayCommand]
    private async Task SelectPhotoCommand(int id)
    {
        var result = await _photoGallery.GetPhotosByIdAsync(id);
        
        if (result.IsError)
        {
            await result.GetError().DisplayErrorAsync();
            
            return;
        }

        var photo = result.GetValue();
        
        await Shell.Current.GoToAsync(nameof(PhotoPage), true, new Dictionary<string, object>()
        {
            {
                "ViewPhotoPair", _imageSourceLoader.LoadViewPhotoPair(photo.Original, photo.Processed)
            }
        });
    }
    
    public async Task LoadPhotosAsync()
    {
        Photos.Clear();
        var photoPairs = await _photoGallery.GetPhotoPairsAsync();
        
        foreach (var photoPair in photoPairs)
        {
            var result = await _photoGallery.GetPhotosAsync(photoPair);
            
            if (result.IsError)
            {
                await result.GetError().DisplayErrorAsync();
                
                continue;
            }

            var photo = result.GetValue();
            var viewPhoto = _imageSourceLoader.LoadViewPhotoPair(
                photo.Original,
                photo.Processed);
            
            Photos.Add(viewPhoto);
        }
    }
}
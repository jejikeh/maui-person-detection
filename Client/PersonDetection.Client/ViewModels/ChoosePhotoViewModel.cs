using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Extensions;
using PersonDetection.Client.Models;
using PersonDetection.Client.Pages;
using PersonDetection.Client.Services;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace PersonDetection.Client.ViewModels;

public partial class ChoosePhotoViewModel(
    PhotoService photoService, 
    IPhotoGallery photoGallery,
    IPlatformImageSourceLoader imageSourceLoader,
    IPlatformFilePicker platformFilePicker) : ObservableObject
{
    [ObservableProperty]
    private ObservableRangeCollection<ViewPhotoPair> _photos = new();
    
    [ObservableProperty]
    private int _imageHeight = (int)Shell.Current.Height / 3;

    [RelayCommand]
    private async Task AddNewPhoto()
    {
        var result = await photoService.NewPhoto();
        if (result.IsError)
        {
            await result.GetError().ToastErrorAsync();
            return;
        }
        
        var photo = result.GetValue();
        Photos.Add(imageSourceLoader.LoadViewPhotoPair(
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
        var result = await photoGallery.GetPhotosByIdAsync(id);
        if (result.IsError)
        {
            await result.GetError().DisplayErrorAsync();
            return;
        }

        var photo = result.GetValue();
        await Shell.Current.GoToAsync(nameof(PhotoPage), true, new Dictionary<string, object>()
        {
            {
                "ViewPhotoPair", imageSourceLoader.LoadViewPhotoPair(photo.Original, photo.Processed)
            }
        });
    }
    
    public async Task LoadPhotosAsync()
    {
        Photos.Clear();
        var photoPairs = await photoGallery.GetPhotoPairsAsync();
        foreach (var photoPair in photoPairs)
        {
            var result = await photoGallery.GetPhotosAsync(photoPair);
            if (result.IsError)
            {
                await result.GetError().DisplayErrorAsync();
                continue;
            }

            var photo = result.GetValue();
            var viewPhoto = imageSourceLoader.LoadViewPhotoPair(
                photo.Original,
                photo.Processed);
            
            Photos.Add(viewPhoto);
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonDetection.Client.Application.Extensions;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;

namespace PersonDetection.Client.Application.ViewModels;

public partial class ChoosePhotoViewModel(
    IPhotoProcessService photoProcessService,
    IProcessedPhotoGallery processedPhotoGallery,
    IPlatformFilePicker platformFilePicker) : ObservableObject
{
    [RelayCommand]
    private async Task AddNewPhoto()
    {
        var filePhoto = await platformFilePicker.PickPhotosAsync();
        if (filePhoto is null)
        {
            // @Incomplete: Handle error here
            return;
        }

        await using var stream = await filePhoto.OpenReadAsync();
        var photoBase64 = stream.ToBase64();
        
        var photoProcessResult = await photoProcessService.ProcessPhoto(
            photoBase64, 
            CancellationToken.None);
        
        var processedPhoto = new ProcessedPhoto()
        {
            Id = photoProcessResult.Id,
            NewPhoto = photoProcessResult.Photo,
            OriginalPhoto = photoBase64,
        };
        
        await processedPhotoGallery.AddAsync(processedPhoto);
    }
}
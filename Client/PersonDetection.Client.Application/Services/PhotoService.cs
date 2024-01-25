using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Services;

public class PhotoService(
    IPhotoProcessService photoProcessService,
    IPhotoGallery photoGallery,
    IPlatformFilePicker platformFilePicker)
{
    public async Task<(Photo Original, Photo Processed)?> NewPhoto()
    {
        var originalPhoto = await platformFilePicker.PickPhotoAsync();
        if (originalPhoto is null)
        {
            return null;
        }

        var processedPhoto = await photoProcessService.ProcessPhotoAsync(
            originalPhoto, 
            CancellationToken.None);
        if (processedPhoto is null)
        {
            return null;
        }
        
        await photoGallery.AddPairAsync(originalPhoto, processedPhoto);
        
        return (originalPhoto, processedPhoto);
    }
}
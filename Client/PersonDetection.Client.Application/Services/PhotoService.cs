using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Application.Services;

public class PhotoService(
    IPhotoProcessService photoProcessService,
    IPhotoGallery photoGallery,
    IPlatformFilePicker platformFilePicker)
{
    public async Task<Result<PhotoTuple, Error>> NewPhotoToGalleryAsync()
    {
        var originalPhoto = await platformFilePicker.PickPhotoAsync();
        
        if (originalPhoto.IsError)
        {
            return originalPhoto.GetError();
        }

        return await Task.Run(async () => await ProcessPhotoToGalleryAsync(originalPhoto));
    }

    public async Task<Result<PhotoTuple, Error>> ProcessPhotoToGalleryAsync(Photo originalPhoto)
    {
        var processedPhoto = await photoProcessService.ProcessPhotoAsync(originalPhoto);
        
        if (processedPhoto.IsError)
        {
            return processedPhoto.GetError();
        }
        
        await photoGallery.AddPairAsync(originalPhoto, processedPhoto);

        return new PhotoTuple()
        {
            Original = originalPhoto,
            Processed = processedPhoto
        };
    }
}
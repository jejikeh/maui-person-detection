using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Application.Services.Implementations;

public class PhotoService(
    IPhotoProcessService _photoProcessService,
    IPhotoGallery _photoGallery,
    IPlatformFilePicker _platformFilePicker) : IPhotoService
{
    public async Task<Result<PhotoTuple, Error>> NewPhotoToGalleryAsync()
    {
        var originalPhoto = await _platformFilePicker.PickPhotoAsync();
        
        if (originalPhoto.IsError)
        {
            return originalPhoto.GetError();
        }
        
        return await ProcessPhotoToGalleryAsync(originalPhoto);
    }

    public async Task<Result<PhotoTuple, Error>> ProcessPhotoToGalleryAsync(Photo originalPhoto)
    {
        var processedPhoto = await _photoProcessService.ProcessPhotoAsync(originalPhoto);
        
        if (processedPhoto.IsError)
        {
            return processedPhoto.GetError();
        }
        
        await _photoGallery.AddPairAsync(originalPhoto, processedPhoto);

        return new PhotoTuple()
        {
            Original = originalPhoto,
            Processed = processedPhoto
        };
    }
}
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.ImageSegmentation.ModelConverter;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class PhotoProcessingService(YoloImageSegmentation _imageProcessing, ModelTypeProvider _modelTypeProvider) : IPhotoProcessingService
{
    public async Task<string> ProcessPhotoAsync(string photo)
    {
        var validatePhoto = ValidatePhoto(photo, out _);
        
        if (!validatePhoto)
        {
            throw new InvalidPhotoException();
        }

        var processedPhoto = await _imageProcessing.SegmentAsync(photo, _modelTypeProvider.ModelType);

        if (processedPhoto is null)
        {
            throw new InvalidPhotoException();
        }

        return processedPhoto;
    }

    public async Task<Photo> ProcessPhotoTransparentAsync(string photo)
    {
        var validatePhoto = ValidatePhoto(photo, out var buffer);

        if (!validatePhoto)
        {
            throw new InvalidPhotoException();
        }
        
        var predictions = _imageProcessing.CalculateSegmentation(ref buffer, _modelTypeProvider.ModelType);
        var imageData = await _imageProcessing.DrawSegmentationAsync(predictions);

        return new Photo()
        {
            Content = imageData
        };
    }

    private static bool ValidatePhoto(string photo, out byte[] buffer)
    {
        if (string.IsNullOrEmpty(photo))
        {
            buffer = Array.Empty<byte>();
            
            return false;
        }
        
        buffer = new byte[photo.Length];
        var isBase64 = Convert.TryFromBase64String(photo, buffer, out var bytesWritten);
        
        return isBase64 && bytesWritten != 0;
    }
}
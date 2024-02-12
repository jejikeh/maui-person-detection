using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Models;
using PersonDetection.ImageSegmentation.ModelConverter;

namespace PersonDetection.Backend.Application.Services;

public class PhotoProcessingService(YoloImageSegmentation imageProcessing)
{
    public async Task<Photo> ProcessPhotoAsync(Photo photo)
    {
        var validatePhoto = ValidatePhoto(photo, out _);
        
        if (!validatePhoto)
        {
            throw new InvalidPhotoException();
        }

        var processedPhoto = await imageProcessing.SegmentAsync(photo.Content);

        if (processedPhoto is null)
        {
            throw new InvalidPhotoException();
        }
        
        return new Photo
        {
            Content = processedPhoto
        };
    }

    public async Task<Photo> ProcessPhotoTransparentAsync(Photo photo)
    {
        var validatePhoto = ValidatePhoto(photo, out var buffer);

        if (!validatePhoto)
        {
            throw new InvalidPhotoException();
        }
        
        var predictions = imageProcessing.CalculateSegmentation(photo.Content);
        var imageData = await imageProcessing.DrawSegmentationAsync(predictions);

        return new Photo()
        {
            Content = imageData
        };
    }

    private static bool ValidatePhoto(Photo photo, out byte[] buffer)
    {
        var content = photo.Content;
        
        if (string.IsNullOrEmpty(content))
        {
            buffer = Array.Empty<byte>();
            
            return false;
        }
        
        buffer = new byte[content.Length];
        var isBase64 = Convert.TryFromBase64String(content, buffer, out var bytesWritten);
        
        return isBase64 && bytesWritten != 0;
    }
}
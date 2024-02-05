using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Models;
using PersonDetection.ImageProcessing;

namespace PersonDetection.Backend.Application.Services;

public class PhotoProcessingService(YoloImageProcessing imageProcessing)
{
    public async Task<ProcessedPhoto> ProcessPhotoAsync(Photo photo)
    {
        var validatePhoto = ValidatePhoto(photo, out _);
        
        if (!validatePhoto)
        {
            throw new InvalidPhotoException();
        }

        var processedPhoto = await imageProcessing.Predict(photo.Content);

        if (processedPhoto is null)
        {
            throw new InvalidPhotoException();
        }
        
        return new ProcessedPhoto
        {
            Content = processedPhoto
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
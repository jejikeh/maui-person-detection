using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Models;
using PersonDetection.Client.Services;

namespace PersonDetection.Client.Platforms.MacCatalyst.Services;

public class MacImageSourceLoader : IPlatformImageSourceLoader
{
    public ViewPhotoPair LoadViewPhotoPair(Photo originalPhoto, Photo processedPhoto)
    {
        return new ViewPhotoPair()
        {
            Id = originalPhoto.Id,
            // Since apps in MacOs is executed in sandbox,
            // there a chance that original photo was loaded from tmp directory and then deleted.
            Original = string.IsNullOrEmpty(originalPhoto.FileUrl) 
                ? ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(originalPhoto.Content)))
                : ImageSource.FromFile(originalPhoto.FileUrl),
            // There a option in ClientConfiguration.cs to not save processed photo as file.
            Processed = string.IsNullOrEmpty(processedPhoto.FileUrl) 
                ? ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(processedPhoto.Content)))
                : ImageSource.FromFile(processedPhoto.FileUrl)
        };
    }
}
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
            // As apps in MacOS are executed within a sandbox,
            // there is a chance that the original photo was loaded from the temporary directory and subsequently deleted.
            Original = string.IsNullOrEmpty(originalPhoto.FileUrl) 
                ? ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(originalPhoto.Content)))
                : ImageSource.FromFile(originalPhoto.FileUrl),
            // There is an option in ClientConfiguration.cs to prevent saving the processed photo as a file.
            Processed = string.IsNullOrEmpty(processedPhoto.FileUrl) 
                ? ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(processedPhoto.Content)))
                : ImageSource.FromFile(processedPhoto.FileUrl)
        };
    }
}
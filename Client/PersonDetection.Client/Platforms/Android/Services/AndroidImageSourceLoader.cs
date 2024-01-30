using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Models;
using PersonDetection.Client.Services;

namespace PersonDetection.Client.Platforms.Android.Services;

public class AndroidImageSourceLoader : IPlatformImageSourceLoader
{
    public ViewPhotoPair LoadViewPhotoPair(Photo originalPhoto, Photo processedPhoto)
    {
        // On Android, ImageSource.FromFile() doesn't work.
        // It may be a problem with permissions, but it seems to me that it's a bug.
        // According to the issues, there are problems with ImageSource.
        // - https://github.com/dotnet/maui/issues/14471
        // - https://github.com/dotnet/maui/issues/14205
        // - https://github.com/dotnet/maui/issues/7074
        // For now, I will use ImageSource.FromStream(), though it may impact performance and memory usage.
        return new ViewPhotoPair()
        {
            Id = originalPhoto.Id,
            Original = ImageSource.FromStream(() => 
                new MemoryStream(Convert.FromBase64String(originalPhoto.Content))),
            Processed = ImageSource.FromStream(() => 
                new MemoryStream(Convert.FromBase64String(processedPhoto.Content))),
        };
    }
}
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Models;
using PersonDetection.Client.Services;

namespace PersonDetection.Client.Platforms.Android.Services;

public class AndroidImageSourceLoader : IPlatformImageSourceLoader
{
    public ViewPhotoPair LoadViewPhotoPair(Photo originalPhoto, Photo processedPhoto)
    {
        // On Android, ImageSource.FromFile() does not work. It could be a permissions issue, but it appears to be a bug.
        // Several issues related to ImageSource have been reported:
        // - https://github.com/dotnet/maui/issues/14471
        // - https://github.com/dotnet/maui/issues/14205
        // - https://github.com/dotnet/maui/issues/7074
        // For the time being, I will use ImageSource.FromStream(), even though it might affect performance and memory usage.
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
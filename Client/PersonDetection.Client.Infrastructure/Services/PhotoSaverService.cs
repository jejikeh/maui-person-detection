using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Options;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Infrastructure.Common.Options;

namespace PersonDetection.Client.Infrastructure.Services;

public class PhotoSaverService(IFileSaver fileSaver, IOptions<PhotoSaverOptions> options)
{
    public async Task UserSavePhotoAsync(Photo photo)
    {
        var decodedImage = Convert.FromBase64String(photo.Content);
        using var stream = new MemoryStream(decodedImage);
        var fileSaverResult = await fileSaver.SaveAsync("image.png", stream);
        
        if (!fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"Failed to save image: {fileSaverResult.Exception.Message}").Show();
            
            return;
        }
        
        await Toast.Make($"Image saved to: {fileSaverResult.FilePath}").Show();
    }

    public async Task CachePhotoAsync(Photo photo)
    {
        var decodedImage = Convert.FromBase64String(photo.Content);
        var filePath = options.Value.ImageCacheDirectory + Guid.NewGuid();
        
        using var stream = new MemoryStream(decodedImage);
        await using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.WriteTo(file);
        
        photo.FileUrl = filePath;
    }
}
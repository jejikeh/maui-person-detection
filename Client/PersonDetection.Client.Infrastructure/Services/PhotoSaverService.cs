using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Infrastructure.Services;

public class PhotoSaverService(IFileSaver fileSaver)
{
    public async Task<string> SavePhotoAsync(Photo photo)
    {
        var decodedImage = Convert.FromBase64String(photo.Content);
        using var stream = new MemoryStream(decodedImage);
        var fileSaverResult = await fileSaver.SaveAsync("image.png", stream);
        if (!fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"Failed to save image: {fileSaverResult.Exception.Message} ").Show();
            return string.Empty;
        }
        
        await Toast.Make($"Image saved to: {fileSaverResult.FilePath}").Show();
        
        return fileSaverResult.FilePath!;
    }
}
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Application.Extensions;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Platforms.Android.Services;

public class AndroidFilePicker : IPlatformFilePicker
{
    public async Task<Result<Photo, Error>> PickPhotoAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions()
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Select photo"
        });

        if (result is null)
        {
            return new Error("No photo was selected");
        }
        
        await using var stream = await result.OpenReadAsync();
        var photo = new Photo
        {
            Content = stream.ToBase64(),
            FileUrl = result.FullPath
        };

        return photo;
    }
}
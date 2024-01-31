using PersonDetection.Client.Application.Extensions;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Common;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace PersonDetection.Client.Platforms.MacCatalyst.Services;

public class MacFilePicker : IPlatformFilePicker
{
    private static readonly string[] SupportedImageFileTypes = ["public.image"];

    private static readonly Dictionary<DevicePlatform, IEnumerable<string>> ImageFileType = new Dictionary<DevicePlatform, IEnumerable<string>>()
        {
            { DevicePlatform.MacCatalyst, SupportedImageFileTypes }
        };

    public async Task<Result<Photo, Error>> PickPhotoAsync()
    {
        var result = await MauiApplication.Current?.Dispatcher.DispatchAsync(async () => 
            await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Photo",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.MacCatalyst, SupportedImageFileTypes }
                })
            }))!;

        if (result is null)
        {
            return new Error(ClientErrorMessages.NoPhotoSelected);
        }

        await using var stream = await result.OpenReadAsync();
        var photo = new Photo
        {
            Content = stream.ToBase64(),
            FileUrl = result!.FullPath
        };

        return photo;
    }
}
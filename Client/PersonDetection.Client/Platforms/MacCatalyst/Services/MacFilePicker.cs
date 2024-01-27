using PersonDetection.Client.Application.Extensions;
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Application.Services;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace PersonDetection.Client.Platforms.MacCatalyst.Services;

public class MacFilePicker : IPlatformFilePicker
{
    private static readonly string[] SupportedImageFileTypes = ["public.image"];

    private static readonly Dictionary<DevicePlatform, IEnumerable<string>> ImageFileType =
        new()
        {
            { DevicePlatform.MacCatalyst, SupportedImageFileTypes }
        };

    public async Task<Result<Photo, Error>> PickPhotoAsync()
    {
        // @Note: Maui FilePicker is not working on MacCatalyst.
        // The result from FilePicker is always null.
        // See these issues:
        // - https://github.com/dotnet/maui/issues/15126
        // - https://github.com/dotnet/maui/issues/11088
        //
        // The last fix in https://github.com/dotnet/maui/pull/13814 doesn't work.
        // @Old: Currently, I am utilizing the FilePickerService from LukeMauiFilePicker.
        // @New: It appears that the DispatchAsync() function is resolving the issue with the Default File Picker,
        // but it is causing a crash in LukeMauiFilePicker.
        var result = await MauiApplication.Current?.Dispatcher.DispatchAsync(async ()
            => await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Photo",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.MacCatalyst, SupportedImageFileTypes }
                })
            }))!;

        if (result is null)
        {
            return new Error("No photo was selected");
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
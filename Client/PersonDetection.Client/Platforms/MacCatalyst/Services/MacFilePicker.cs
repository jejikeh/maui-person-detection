using LukeMauiFilePicker;
using PersonDetection.Client.Application.Extensions;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Platforms.MacCatalyst.Services;

public class MacFilePicker(IFilePickerService filePickerService) : IPlatformFilePicker
{
    private static readonly string[] SupportedImageFileTypes = ["public.image"];

    private static readonly  Dictionary<DevicePlatform, IEnumerable<string>> ImageFileType = 
        new Dictionary<DevicePlatform, IEnumerable<string>>()
        {
            { DevicePlatform.MacCatalyst, SupportedImageFileTypes }
        };

    public async Task<Photo?> PickPhotoAsync()
    {
        // @Note: Maui FilePicker is not working on MacCatalyst.
        // The result from FilePicker is always null.
        // See these issues:
        // - https://github.com/dotnet/maui/issues/15126
        // - https://github.com/dotnet/maui/issues/11088
        //
        // The last fix in https://github.com/dotnet/maui/pull/13814 doesn't work.
        // For now, I will use FilePickerService from LukeMauiFilePicker.
        try
        {
            var result = await filePickerService.PickFileAsync(
                "Select Photo",
                ImageFileType)
                .ConfigureAwait(false);
            
            if (result is null)
            {
                return null;
            }
        
            await using var stream = await result.OpenReadAsync();
        
            var photo = new Photo
            {
                Content = stream.ToBase64(),
                FileUrl = result.FileResult!.FullPath
            };
        
            return photo;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            return null;
        }
    }
}
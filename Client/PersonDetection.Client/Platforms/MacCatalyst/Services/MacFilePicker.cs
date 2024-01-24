using PersonDetection.Client.Application.Services;

namespace PersonDetection.Client.Platforms.MacCatalyst.Services;

public class MacFilePicker : IPlatformFilePicker
{
    private static readonly string[] SupportedImageFileTypes = ["public.image"];

    private static readonly FilePickerFileType ImageFileType = new FilePickerFileType(
        new Dictionary<DevicePlatform, IEnumerable<string>>()
        {
            { DevicePlatform.MacCatalyst, SupportedImageFileTypes }
        });

    public Task<FileResult?> PickPhotosAsync()
    {
        return FilePicker.PickAsync(new PickOptions()
        {
            FileTypes = ImageFileType,
            PickerTitle = "Select photo"
        });
    }
}
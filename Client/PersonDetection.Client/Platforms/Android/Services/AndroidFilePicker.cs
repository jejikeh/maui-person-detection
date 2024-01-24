using PersonDetection.Client.Application.Services;

namespace PersonDetection.Client.Platforms.Android.Services;

public class AndroidFilePicker : IPlatformFilePicker
{
    public  Task<FileResult?> PickPhotosAsync()
    {
        return FilePicker.PickAsync(new PickOptions()
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Select photo"
        });
    }
}
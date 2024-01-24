namespace PersonDetection.Client.Application.Services;

public interface IPlatformFilePicker
{ 
    public Task<FileResult?> PickPhotosAsync();
}
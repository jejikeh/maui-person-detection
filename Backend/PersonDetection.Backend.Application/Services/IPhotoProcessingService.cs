namespace PersonDetection.Backend.Application.Services;

public interface IPhotoProcessingService
{
    public Task<string> ProcessPhotoAsync(string base64Image);
    public IAsyncEnumerable<string> ProcessPhotosStreamAsync(IAsyncEnumerable<string> photos);
}
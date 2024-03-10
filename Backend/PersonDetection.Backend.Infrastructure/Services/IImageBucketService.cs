namespace PersonDetection.Backend.Infrastructure.Services;

public interface IImageBucketService
{
    public Task SavePhotoAsync(string photoName, string photo);
    public Task<List<string>> GetPhotosAsync(int page, int count);
}
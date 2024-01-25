using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Services;

public interface IPhotoGallery
{
    public Task<List<PhotoPair>> GetPhotoPairsAsync();
    public Task<(Photo Original, Photo Processed)?> GetPhotosAsync(PhotoPair photoPair);
    public Task<(Photo Original, Photo Processed)?> GetPhotosByIdAsync(int id);
    public Task AddPairAsync(Photo originalPhoto, Photo processedPhoto);
    public Task DeletePairAsync(Photo photo);
}
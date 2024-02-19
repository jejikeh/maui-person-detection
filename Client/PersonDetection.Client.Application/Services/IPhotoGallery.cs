using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Application.Services;

public interface IPhotoGallery
{
    public Task<List<PhotoPair>> GetPhotoPairsAsync();
    public Task<Result<PhotoTuple, Error>> GetPhotosAsync(PhotoPair photoPair);
    public Task<Result<PhotoTuple, Error>> GetPhotosByIdAsync(int id);
    public Task AddPairAsync(Photo originalPhoto, Photo processedPhoto);
    public Task<Result<string, Error>> DeletePairAsync(Photo photo);
}
using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Application.Services;

public interface IPhotoService
{
    public Task<Result<PhotoTuple, Error>> NewPhotoToGalleryAsync();
    public Task<Result<PhotoTuple, Error>> ProcessPhotoToGalleryAsync(Photo originalPhoto);
}
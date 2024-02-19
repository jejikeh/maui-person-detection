using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;

namespace PersonDetection.Client.Application.Services;

public interface IPhotoProcessService
{
    public Task<Result<Photo, Error>> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default);
}
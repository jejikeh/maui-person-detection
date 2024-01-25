using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Services;

public interface IPhotoProcessService
{
    public Task<Photo?> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default);
}
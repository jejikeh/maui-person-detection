using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;

namespace PersonDetection.Client.Infrastructure.Services;

public class PhotoProcessYoloService : IPhotoProcessService
{
    public Task<Photo?> ProcessPhotoAsync(Photo originalPhoto, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => originalPhoto, cancellationToken);
    }
}
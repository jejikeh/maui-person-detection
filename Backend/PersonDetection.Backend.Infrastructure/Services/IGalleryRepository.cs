using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Infrastructure.Models;

namespace PersonDetection.Backend.Infrastructure.Services;

public interface IGalleryRepository
{
    public Task SavePhotoAsync(PhotoInBucket photoInBucket, CancellationToken cancellationToken);
    public Task<List<PhotoInBucket>> GetPhotosAsync(string userId, int page, int size);
    public Task DeletePhotoAsync(string userId, int photoId, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
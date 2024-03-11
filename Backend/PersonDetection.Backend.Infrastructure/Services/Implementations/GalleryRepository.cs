using Microsoft.EntityFrameworkCore;
using PersonDetection.Backend.Infrastructure.Models;

namespace PersonDetection.Backend.Infrastructure.Services.Implementations;

public class GalleryRepository(PersonDetectionDbContext _detectionDbContext) : IGalleryRepository
{
    public async Task SavePhotoAsync(PhotoInBucket photoInBucket, CancellationToken cancellationToken)
    {
        await _detectionDbContext.Photos.AddAsync(photoInBucket, cancellationToken);
    }

    public async Task<List<PhotoInBucket>> GetPhotosAsync(string userId, int page, int size)
    {
        return await _detectionDbContext.Photos
            .Where(x => x.OwnerId == userId)
            .OrderBy(x => x.Id)
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task DeletePhotoAsync(string userId, int photoId, CancellationToken cancellationToken)
    {
        _detectionDbContext.Remove(await _detectionDbContext.Photos.SingleAsync(photo =>
            photo.Id == photoId && photo.OwnerId == userId, cancellationToken));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _detectionDbContext.SaveChangesAsync(cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using PersonDetection.Backend.Infrastructure.Models;

namespace PersonDetection.Backend.Infrastructure.Services.Implementations;

public class GalleryRepository(PersonDetectionDbContext _detectionDbContext) : IGalleryRepository
{
    public async Task SavePhotoAsync(PhotoInBucket photoInBucket)
    {
        await _detectionDbContext.Photos.AddAsync(photoInBucket);
    }

    public async Task<List<PhotoInBucket>> GetPhotosAsync(Guid userId, int page, int size)
    {
        return await _detectionDbContext.Photos
            .Where(x => x.OwnerId == userId)
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }
}
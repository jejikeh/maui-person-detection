using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Infrastructure.Models;

namespace PersonDetection.Backend.Infrastructure.Services;

public interface IGalleryRepository
{
    public Task SavePhotoAsync(PhotoInBucket photoInBucket);
    public Task<List<PhotoInBucket>> GetPhotosAsync(Guid userId, int page, int size);
}
using System.Security.Claims;

namespace PersonDetection.Backend.Application.Services;

public interface IGalleryService
{
    public Task SavePhotoAsync(ClaimsPrincipal claims, string photo);
    public Task<List<string>> GetPhotosAsync(ClaimsPrincipal claims, int page, int size);
}
using System.Security.Claims;
using PersonDetection.Backend.Application.Common.Models.Dtos;

namespace PersonDetection.Backend.Application.Services;

public interface IGalleryService
{
    public Task SavePhotoAsync(ClaimsPrincipal claims, string photo, CancellationToken cancellationToken);
    public Task DeletePhotoAsync(ClaimsPrincipal claims, int photoId, CancellationToken cancellationToken);
    public Task<List<GalleryPhoto>> GetPhotosAsync(ClaimsPrincipal claims, int page, int size);
}
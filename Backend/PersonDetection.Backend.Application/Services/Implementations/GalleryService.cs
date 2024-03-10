using System.Security.Authentication;
using System.Security.Claims;
using PersonDetection.Backend.Infrastructure.Services;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class GalleryService(IImageBucketService imageBucketService) : IGalleryService
{
    public async Task SavePhotoAsync(ClaimsPrincipal claims, string photo)
    {
        var id = claims.FindFirst(ClaimTypes.NameIdentifier);

        if (id is null)
        {
            throw new InvalidCredentialException();
        }
        
        var photoName = $"{id.Value}/{Guid.NewGuid()}";
        
        await imageBucketService.SavePhotoAsync(photoName, photo);
    }

    public Task<List<string>> GetPhotosAsync(ClaimsPrincipal claims, int page, int size)
    {
        throw new NotImplementedException();
    }
}
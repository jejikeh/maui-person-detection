using System.Security.Claims;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models.Dtos;
using PersonDetection.Backend.Infrastructure.Models;
using PersonDetection.Backend.Infrastructure.Services;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class GalleryService(IImageBucketService _imageBucketService, IGalleryRepository _galleryRepository) : IGalleryService
{
    public async Task SavePhotoAsync(ClaimsPrincipal claims, string photo, CancellationToken cancellationToken)
    {
        var id = GetIdFromClaims(claims);

        var photoName = $"{id.Value}/{Guid.NewGuid()}";

        await _galleryRepository.SavePhotoAsync(new PhotoInBucket()
        {
            OwnerId = Guid.Parse(id.Value).ToString(),
            PhotoName = photoName
        }, cancellationToken);

        await _imageBucketService.SavePhotoAsync(photoName, photo);
        await _galleryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePhotoAsync(ClaimsPrincipal claims, int photoId, CancellationToken cancellationToken)
    {
        var id = GetIdFromClaims(claims);

        await _galleryRepository.DeletePhotoAsync(
            Guid.Parse(id.Value).ToString(),
            photoId,
            cancellationToken);

        await _galleryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<GalleryPhoto>> GetPhotosAsync(ClaimsPrincipal claims, int page, int size)
    {
        var id = GetIdFromClaims(claims);

        var photosInBucket = await _galleryRepository.GetPhotosAsync(Guid.Parse(id.Value).ToString(), page, size);

        var photos = new List<GalleryPhoto>();

        foreach (var photo in photosInBucket)
        {
            photos.Add(new GalleryPhoto(photo.Id, await _imageBucketService.GetPhotoAsync(photo.PhotoName)));
        }

        return photos;
    }

    private static Claim GetIdFromClaims(ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.NameIdentifier) ?? throw new InvalidCredentialsException();

        return id;
    }
}
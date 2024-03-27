using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class GalleryEndpoints
{
    public static async Task<IResult> SaveToGalleryHandlerAsync([FromBody] Photo photo,
        ClaimsPrincipal claimsPrincipal,
        [FromServices] IPhotoProcessingService photoProcessingService,
        [FromServices] IGalleryService galleryService,
        CancellationToken cancellationToken)
    {
        var photoContent = await photoProcessingService.ProcessPhotoAsync(photo.Content);
        
        await galleryService.SavePhotoAsync(claimsPrincipal, photoContent, cancellationToken);
        
        return Results.Created();
    }

    public static async Task<IResult> GetPhotosHandlerAsync(
        ClaimsPrincipal claimsPrincipal,
        [FromQuery] int page,
        [FromQuery] int size,
        [FromServices] IGalleryService galleryService)
    {
        var photos = await galleryService.GetPhotosAsync(claimsPrincipal, page, size);
        
        return Results.Ok(photos);
    }

    public static async Task<IResult> DeletePhotoHandlerAsync(
        ClaimsPrincipal claimsPrincipal,
        [FromQuery] int photoId,
        [FromServices] IGalleryService galleryService,
        CancellationToken cancellationToken)
    {
        await galleryService.DeletePhotoAsync(claimsPrincipal, photoId, cancellationToken);
        
        return Results.Ok();
    }
}
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
        [FromServices] IGalleryService galleryService)
    {
        var photoContent = await photoProcessingService.ProcessPhotoAsync(photo.Content);
        
        await galleryService.SavePhotoAsync(claimsPrincipal, photoContent);
        
        return Results.Ok();
    }
}
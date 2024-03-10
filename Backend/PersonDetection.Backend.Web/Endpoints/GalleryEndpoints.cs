using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class GalleryEndpoints
{
    public static async Task<IResult> SaveToGalleryHandlerAsync([FromBody] Photo photo,
        [FromServices] IPhotoProcessingService photoProcessingService)
    {
        await photoProcessingService.ProcessAndSavePhotoAsync(photo.Content);
        
        return Results.Ok();
    }
}
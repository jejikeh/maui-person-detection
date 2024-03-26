using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Common.Models;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class PhotoEndpoint
{
    public static async Task<IResult> HandlerAsync([FromBody] Photo photo,
        [FromServices] IPhotoProcessingService photoProcessingService)
    {
        return Results.Ok(new Photo()
        {
            Content = await photoProcessingService.ProcessPhotoAsync(photo.Content)
        });
    }
}
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Models;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class PhotoEndpoint
{
    public static async Task<IResult> HandlerAsync([FromBody] Photo photo, [FromServices] PhotoProcessingService photoProcessingService)
    {
        return Results.Ok(await photoProcessingService.ProcessPhotoAsync(photo));
    }
}
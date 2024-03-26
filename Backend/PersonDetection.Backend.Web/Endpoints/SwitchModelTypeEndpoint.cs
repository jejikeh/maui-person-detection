using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Application.Services.Implementations;

namespace PersonDetection.Backend.Web.Endpoints;

public static class SwitchModelTypeEndpoint
{
    public static async Task<IResult> HandlerAsync([FromServices] ModelTypeProvider modelTypeProvider)
    {
        return Results.Ok(modelTypeProvider.SwitchModelType().ToString());
    }
}
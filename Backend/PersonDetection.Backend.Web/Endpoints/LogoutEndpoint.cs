using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class LogoutEndpoint
{
    public static async Task<IResult> Handler([FromServices] IAuthorizationService authorizationService)
    {
        return await authorizationService.LogoutAsync();
    }
}
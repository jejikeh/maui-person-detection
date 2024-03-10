using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class IdentifyEndpoint
{
    public static Task<IResult> Handler(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] IAuthorizationService authorizationService)
    {
        return authorizationService.IdentifyAsync(claimsPrincipal);
    }
}
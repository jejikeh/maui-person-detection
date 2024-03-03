using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class IdentifyEndpoint
{
    public static IResult Handler(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] IAuthorizationService authorizationService)
    {
        return authorizationService.Identify(claimsPrincipal);
    }
}
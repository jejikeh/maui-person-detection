using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Common.Models.Requests.Login;
using PersonDetection.Backend.Application.Common.Models.Requests.Register;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class IdentityEndpoints
{
    public static Task<IResult> IdentifyHandlerAsync(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] IAuthorizationService authorizationService)
    {
        return authorizationService.IdentifyAsync(claimsPrincipal);
    }
    
    public static async Task<IResult> LoginHandlerAsync(
        [FromBody] LoginRequest loginRequest, 
        [FromServices] IAuthorizationService authorizationService, 
        [FromServices] IValidator<LoginRequest> validator)
    {
        return await authorizationService.LoginAsync(loginRequest, validator);
    }
    
    public static async Task<IResult> LogoutHandlerAsync([FromServices] IAuthorizationService authorizationService)
    {
        return await authorizationService.LogoutAsync();
    }
    
    public static async Task<IResult> RegisterHandlerAsync(
        [FromBody] RegisterRequest registerRequest,
        [FromServices] IAuthorizationService authorizationService,
        [FromServices] IValidator<RegisterRequest> validator)
    {
        return await authorizationService.RegisterAsync(registerRequest, validator);
    }
}
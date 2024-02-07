using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Services;
using LoginRequest = PersonDetection.Backend.Application.Common.Models.Requests.Login.LoginRequest;

namespace PersonDetection.Backend.Web.Endpoints;

public static class LoginEndpoint
{
    public static async Task<IResult> Handler(
        [FromBody] LoginRequest loginRequest, 
        [FromServices] IAuthorizationService authorizationService, 
        [FromServices] IValidator<LoginRequest> validator)
    {
        return await authorizationService.LoginAsync(loginRequest, validator);
    }
}
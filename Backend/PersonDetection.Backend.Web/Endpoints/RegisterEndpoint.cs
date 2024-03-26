using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Common.Models.Requests.Register;
using PersonDetection.Backend.Application.Services;

namespace PersonDetection.Backend.Web.Endpoints;

public static class RegisterEndpoint
{
    public static async Task<IResult> HandlerAsync(
        [FromBody] RegisterRequest registerRequest,
        [FromServices] IAuthorizationService authorizationService,
        [FromServices] IValidator<RegisterRequest> validator)
    {
        return await authorizationService.RegisterAsync(registerRequest, validator);
    }
}
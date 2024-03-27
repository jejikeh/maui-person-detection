using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using PersonDetection.Backend.Application.Common.Models.Requests.Login;
using PersonDetection.Backend.Application.Common.Models.Requests.Register;

namespace PersonDetection.Backend.Application.Services;

public interface IAuthorizationService
{
    public Task<IResult> RegisterAsync(RegisterRequest registerRequest, IValidator<RegisterRequest> validator);
    public Task<IResult> LoginAsync(LoginRequest loginRequest, IValidator<LoginRequest> validator);
    public Task<IResult> IdentifyAsync(ClaimsPrincipal claimsPrincipal);
    public Task<IResult> LogoutAsync();
}
using FluentValidation;
using PersonDetection.Backend.Application.Common.Models.Requests;
using PersonDetection.Backend.Application.Common.Models.Responses;

namespace PersonDetection.Backend.Application.Services.Authorization;

public interface IAuthorizationService
{
    public Task<LoginTokens> RegisterAsync(RegisterRequest registerRequest, IValidator<RegisterRequest> validator);
}
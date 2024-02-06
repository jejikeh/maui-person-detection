using PersonDetection.Backend.Application.Common.Models.Responses;
using PersonDetection.Backend.Infrastructure.Models.Identity;

namespace PersonDetection.Backend.Application.Services.Authorization;

public interface ILoginTokensProvider
{
    public Task<LoginTokens> GenerateLoginTokensAsync(User user);
    public Task<string> GenerateJwtTokenAsync(User user);
}
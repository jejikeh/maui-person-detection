using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PersonDetection.Backend.Application.Common.Models.Responses;
using PersonDetection.Backend.Application.Common.Options;
using PersonDetection.Backend.Infrastructure.Models.Identity;
using PersonDetection.Backend.Infrastructure.Services.Repositories;

namespace PersonDetection.Backend.Application.Services.Authorization.Implementations;

public class LoginTokensProvider(
    IOptions<LoginTokensProviderOptions> options, 
    IRefreshTokenRepository refreshTokenRepository,
    ISecurityKeyService securityKeyService) : ILoginTokensProvider
{
    public async Task<LoginTokens> GenerateLoginTokensAsync(User user)
    {
        var refreshToken = new RefreshToken(
            user.Id, 
            DateTime.UtcNow.AddSeconds(options.Value.RefreshTokenTtl),
            options.Value.ContentLength);

        await refreshTokenRepository.AddAsync(refreshToken);

        return new LoginTokens(
            refreshToken.Content, 
            await GenerateJwtTokenAsync(user), 
            user.UserName ?? string.Empty, 
            user.Email ?? string.Empty, 
            refreshToken.Id);
    }

    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        var key = await securityKeyService.ProvideRsaSecurityKeyAsync();
        var tokenHandler = new JsonWebTokenHandler();

        return tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Expires = null,
            Issuer = options.Value.Host,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
            Subject = new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            })
        });
    }
}
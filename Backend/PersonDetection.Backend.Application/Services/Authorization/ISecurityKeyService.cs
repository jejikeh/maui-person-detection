using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace PersonDetection.Backend.Application.Services.Authorization;

public interface ISecurityKeyService
{
    public Task InitializeAsync();
    public Task<RSA> ProvideRsaAsync();
    public Task<RsaSecurityKey> ProvideRsaSecurityKeyAsync();
    public Task<JsonWebKey> ProvideJsonWebKeyAsync();
}
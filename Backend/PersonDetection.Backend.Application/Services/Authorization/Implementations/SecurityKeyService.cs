using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonDetection.Backend.Application.Common.Options;

namespace PersonDetection.Backend.Application.Services.Authorization.Implementations;

public class SecurityKeyService(IOptions<SecurityKeyServiceOptions> options) : ISecurityKeyService
{
    private RSA? _rsa;
    private RsaSecurityKey? _rsaSecurityKey;

    public async Task InitializeAsync()
    {
        var rsa = RSA.Create();

        if (options.Value.GenerateNewKey)
        {
            var privateKey = rsa.ExportRSAPrivateKey();
            await File.WriteAllBytesAsync(AppContext.BaseDirectory + options.Value.RsaPath, privateKey);
        }
        else
        {
            rsa.ImportRSAPrivateKey(await File.ReadAllBytesAsync(AppContext.BaseDirectory + options.Value.RsaPath), out _);
        }

        _rsa = rsa;
        _rsaSecurityKey = new RsaSecurityKey(_rsa);
    }

    public async Task<RSA> ProvideRsaAsync()
    {
        if (_rsa is null)
        {
            await InitializeAsync();
        }
        
        return _rsa ?? throw new NullReferenceException("RsaKey is not initialized");
    }

    public async Task<RsaSecurityKey> ProvideRsaSecurityKeyAsync()
    {
        if (_rsaSecurityKey is null)
        {
            await InitializeAsync();
        }
        
        return _rsaSecurityKey ?? throw new NullReferenceException("RsaSecurityKey is not initialized");
    }

    public async Task<JsonWebKey> ProvideJsonWebKeyAsync()
    {
        return JsonWebKeyConverter.ConvertFromRSASecurityKey(await ProvideRsaSecurityKeyAsync());
    }
}
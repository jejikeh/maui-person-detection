using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PersonDetection.Backend.Application.Common.Options;
using PersonDetection.Backend.Application.Services.Authorization.Implementations;

namespace PersonDetection.Backend.Application.Tests.Services;

public class SecurityKeyServiceTests
{
    private readonly Mock<IOptions<SecurityKeyServiceOptions>> _securityKeyServiceOptions;

    public SecurityKeyServiceTests()
    {
        _securityKeyServiceOptions = new Mock<IOptions<SecurityKeyServiceOptions>>();
        _securityKeyServiceOptions.Setup(keyService => keyService.Value)
            .Returns(new SecurityKeyServiceOptions()
            {
                RsaPath = "rsa.key"
            });
    }
     
    [Fact]
    public async void GivenRsa_WhenProvideRsaSecurityKey_ThenReturnsRsaSecurityKey()
    {
        // Arrange
        var securityKeyService = new SecurityKeyService(_securityKeyServiceOptions.Object);
        
        // Act
        var key = await securityKeyService.ProvideRsaSecurityKeyAsync();
        
        // Assert
        key.Should().NotBeNull();

        var rsa = await securityKeyService.ProvideRsaAsync();
        key.Should().BeEquivalentTo(new RsaSecurityKey(rsa));
    }

    [Fact]
    public async void WhenInitialize_ThenGeneratesKeysAndSavesThem()
    {
        // Arrange
        var securityKeyService = new SecurityKeyService(_securityKeyServiceOptions.Object);

        // Act
        var key = await securityKeyService.ProvideRsaAsync();
        
        // Assert
        var file = await File.ReadAllTextAsync(AppContext.BaseDirectory + _securityKeyServiceOptions.Object.Value.RsaPath);
        file.Should().NotBeNullOrEmpty();
        
        var fileRsa = RSA.Create();
        fileRsa.ImportRSAPrivateKey(await File.ReadAllBytesAsync(AppContext.BaseDirectory + _securityKeyServiceOptions.Object.Value.RsaPath), out _);
        
        key.ExportRSAPrivateKey().Should().BeEquivalentTo(fileRsa.ExportRSAPrivateKey());
    }

    [Fact]
    public async void WhenProvideJsonWebKey_ThenReturnsJsonWebKey()
    {
        // Arrange
        var securityKeyService = new SecurityKeyService(_securityKeyServiceOptions.Object);
        
        // Act
        var jsonWebKey = await securityKeyService.ProvideJsonWebKeyAsync();
        
        // Assert
        jsonWebKey.Should().NotBeNull();
    }
}
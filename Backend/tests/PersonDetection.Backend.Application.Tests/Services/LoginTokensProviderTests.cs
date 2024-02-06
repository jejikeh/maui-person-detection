using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PersonDetection.Backend.Application.Common.Models.Responses;
using PersonDetection.Backend.Application.Common.Options;
using PersonDetection.Backend.Application.Services.Authorization;
using PersonDetection.Backend.Application.Services.Authorization.Implementations;
using PersonDetection.Backend.Application.Tests.TestData;
using PersonDetection.Backend.Infrastructure.Models.Identity;
using PersonDetection.Backend.Infrastructure.Services.Repositories;

namespace PersonDetection.Backend.Application.Tests.Services;

public class LoginTokensProviderTests
{
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Mock<IOptions<LoginTokensProviderOptions>> _loginTokensProviderOptions;
    private readonly Mock<ISecurityKeyService> _securityKeyService;
    
    public LoginTokensProviderTests()
    {
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _securityKeyService = new Mock<ISecurityKeyService>();
        
        _loginTokensProviderOptions = new Mock<IOptions<LoginTokensProviderOptions>>();
        _loginTokensProviderOptions.Setup(tokens => tokens.Value).Returns(new LoginTokensProviderOptions
        {
            RefreshTokenTtl = 10,
            ContentLength = 16
        });
    }

    [Fact]
    public async void GivenValidUser_WhenGenerateAsync_ThenReturnsLoginTokens()
    {
        // Arrange
        var user = RandomData.GenerateUser();

        _securityKeyService.Setup(securityKeyService =>
                securityKeyService.ProvideRsaSecurityKeyAsync())
            .ReturnsAsync(new RsaSecurityKey(RSA.Create()));
        
        var service = new LoginTokensProvider(
            _loginTokensProviderOptions.Object, 
            _refreshTokenRepository.Object,
            _securityKeyService.Object);
        
        
        // Act
        var result = await service.GenerateLoginTokensAsync(user);
        
        // Assert
        _refreshTokenRepository.Verify(repository => repository.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
        _securityKeyService.Verify(keyService => keyService.ProvideRsaSecurityKeyAsync(), Times.Once);

        result.Should()
            .Match<LoginTokens>(tokens => tokens.UserName == user.UserName && tokens.Email == user.Email);
        
        result.Should().Match<LoginTokens>(tokens => tokens.RefreshToken.Length == _loginTokensProviderOptions.Object.Value.ContentLength);
    }
}
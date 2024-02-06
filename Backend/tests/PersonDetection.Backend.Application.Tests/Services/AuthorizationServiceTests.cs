using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models.Requests;
using PersonDetection.Backend.Application.Common.Models.Requests.Validations;
using PersonDetection.Backend.Application.Common.Models.Responses;
using PersonDetection.Backend.Application.Common.Options;
using PersonDetection.Backend.Application.Services.Authorization;
using PersonDetection.Backend.Application.Services.Authorization.Implementations;
using PersonDetection.Backend.Infrastructure.Models.Identity;
using ValidationException = FluentValidation.ValidationException;

namespace PersonDetection.Backend.Application.Tests.Services;

public class AuthorizationServiceTests
{
    private readonly Mock<IOptions<AuthorizationModelOptions>> _authorizationModelOptions;
    private readonly IValidator<RegisterRequest> _loginRequestValidator;
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<ILoginTokensProvider> _loginTokensProvider;
    private readonly Faker _faker = new Faker();

    public AuthorizationServiceTests()
    {
        _authorizationModelOptions = new Mock<IOptions<AuthorizationModelOptions>>();
        _authorizationModelOptions.Setup(options => options.Value).Returns(new AuthorizationModelOptions()
        {
            MinimalPasswordLength = 5
        });

        _loginRequestValidator = new LoginRequestValidator(_authorizationModelOptions.Object);
        
        var store = new Mock<IUserStore<User>>();
        _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _userManager.Object.UserValidators.Add(new UserValidator<User>());
        _userManager.Object.PasswordValidators.Add(new PasswordValidator<User>());
        
        _loginTokensProvider = new Mock<ILoginTokensProvider>();
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(null, "test", "test")]
    [InlineData("nick", null, "123456")]
    [InlineData("", "hello@gmail.com", null)]
    public async void GivenInValidUserCredentials_WhenRegisterUserAsync_ThenThrowsValidationExceptions(string userName, string email,
        string password)
    {
        // Arrange
        var loginRequest = new RegisterRequest(userName, email, password);
        var service = new AuthorizationService(_userManager.Object, _loginTokensProvider.Object);

        // Act
        var act = async () => await service.RegisterAsync(loginRequest, _loginRequestValidator);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async void GivenAlreadyRegisteredUser_WhenRegisterUserAsync_ThenThrowsInvalidCredentialsExceptions()
    {
        // Arrange
        var userName = _faker.Internet.UserName();
        var email = _faker.Internet.Email();
        
        _userManager.Setup(userManager => 
                userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        var loginRequest = new RegisterRequest(userName, email, _faker.Internet.Password());
        var service = new AuthorizationService(_userManager.Object, _loginTokensProvider.Object);
        
        // Act
        var act = async () => await service.RegisterAsync(loginRequest, _loginRequestValidator);
        
        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async void GivenLoginRequestUser_WhenRegisterUserAsync_ThenReturnsLoginTokens()
    {
        // Arrange
        var userName = _faker.Internet.UserName();
        var email = _faker.Internet.Email();
        
        _userManager.Setup(userManager => 
            userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        _loginTokensProvider.Setup(provider => 
            provider.GenerateLoginTokensAsync(It.IsAny<User>()))
            .ReturnsAsync(new LoginTokens(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), userName, email, Guid.NewGuid()));
        
        var loginRequest = new RegisterRequest(userName, email, _faker.Internet.Password());
        var service = new AuthorizationService(_userManager.Object, _loginTokensProvider.Object);
        
        // Act
        var act = async () => await service.RegisterAsync(loginRequest, _loginRequestValidator);
        
        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
}
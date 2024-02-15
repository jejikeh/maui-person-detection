using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models.Requests.Login;
using PersonDetection.Backend.Application.Common.Models.Requests.Register;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Application.Services.Implementations;
using PersonDetection.Backend.Application.Tests.Fakes;
using PersonDetection.Backend.Infrastructure.Common.Options;
using LoginRequest = PersonDetection.Backend.Application.Common.Models.Requests.Login.LoginRequest;
using RegisterRequest = PersonDetection.Backend.Application.Common.Models.Requests.Register.RegisterRequest;
using ValidationException = FluentValidation.ValidationException;

namespace PersonDetection.Backend.Application.Tests.Services;

public class AuthorizationServiceTests
{
    private readonly IValidator<RegisterRequest> _registerRequestValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly Mock<FakeUserManager> _userManager;
    private readonly Mock<FakeSignInManager> _signInManager;
    private readonly Faker _faker = new Faker();

    public AuthorizationServiceTests()
    {
        Mock<IOptions<IdentityModelOptions>> identityModelOptions = new();
        identityModelOptions.Setup(options => options.Value).Returns(new IdentityModelOptions());

        _registerRequestValidator = new RegisterRequestValidator(identityModelOptions.Object);
        _loginRequestValidator = new LoginRequestValidator(identityModelOptions.Object);
        
        _userManager = new Mock<FakeUserManager>();
        _signInManager = new Mock<FakeSignInManager>();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "test")]
    [InlineData("nick", "")]
    [InlineData("", null)]
    public async void GivenInValidUserCredentials_WhenRegisterUserAsync_ThenThrowsValidationExceptions(string userName, string password)
    {
        // Arrange
        _userManager.Setup(manager => manager.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(() => IdentityResult.Success);
        
        var loginRequest = new RegisterRequest(userName, _faker.Internet.Email(), password);
        var service = new AuthorizationService(_userManager.Object, _signInManager.Object);

        // Act
        var act = async () => await service.RegisterAsync(loginRequest, _registerRequestValidator);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
    
    [Fact]
    public async void GivenAlreadyRegisteredUser_WhenRegisterUserAsync_ThenThrowsInvalidCredentialsExceptions()
    {
        // Arrange
        var userName = _faker.Internet.UserName();
        
        _userManager.Setup(userManager => 
                userManager.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        var loginRequest = new RegisterRequest(userName, _faker.Internet.Email(), _faker.Internet.Password());
        var service = new AuthorizationService(_userManager.Object, _signInManager.Object);
        
        // Act
        var act = async () => await service.RegisterAsync(loginRequest, _registerRequestValidator);
        
        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async void GivenRegisterRequestUser_WhenRegisterUserAsync_ThenReturnsOk()
    {
        // Arrange
        var userName = _faker.Internet.UserName();
        
        _userManager.Setup(userManager => 
            userManager.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var loginRequest = new RegisterRequest(userName, _faker.Internet.Email(), _faker.Internet.Password());
        var service = new AuthorizationService(_userManager.Object, _signInManager.Object);
        
        // Act
        var act = async () => await service.RegisterAsync(loginRequest, _registerRequestValidator);
        
        // Assert
        await act.Should().NotThrowAsync();
        _signInManager.Verify(s => 
            s.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async void GivenLoginRequestUser_WhenLoginUserAsync_ThenReturnsOk()
    {
        // Arrange
        _signInManager.Setup(manager =>
                manager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(() => SignInResult.Success);
        
        var service = new AuthorizationService(_userManager.Object, _signInManager.Object);
        
        // Act
        var act = async () => await service.LoginAsync(new LoginRequest(_faker.Internet.UserName(), _faker.Internet.Password()), _loginRequestValidator);
        
        // Assert
        await act.Should().NotThrowAsync();
    }
}
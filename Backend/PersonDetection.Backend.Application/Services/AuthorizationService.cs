using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models.Requests.Login;
using PersonDetection.Backend.Application.Common.Models.Requests.Register;

namespace PersonDetection.Backend.Application.Services;

public class AuthorizationService(
    UserManager<IdentityUser> userManager, 
    SignInManager<IdentityUser> signInManager) : IAuthorizationService
{
    public async Task<IResult> RegisterAsync(RegisterRequest registerRequest, IValidator<RegisterRequest> validator)
    {
        await ValidateModelAsync(registerRequest, validator);

        var user = new IdentityUser(registerRequest.UserName)
        {
            Email = registerRequest.Email
        };
        var createUserResult = await userManager.CreateAsync(user, registerRequest.Password);

        if (!createUserResult.Succeeded)
        {
            throw new InvalidCredentialsException();
        }
        
        await signInManager.SignInAsync(user, true);
        
        return Results.Ok();
    }

    public async Task<IResult> LoginAsync(LoginRequest loginRequest, IValidator<LoginRequest> validator)
    {
        await ValidateModelAsync(loginRequest, validator);
     
        var result = await signInManager.PasswordSignInAsync(
            loginRequest.UserName, 
            loginRequest.Password, 
            true, 
            false);
        
        return result.Succeeded ? Results.Ok() : Results.Unauthorized();
    }

    public async Task<IResult> LogoutAsync()
    {
        await signInManager.SignOutAsync();
        
        return Results.Ok();
    }

    private static Task<ValidationResult> ValidateModelAsync<T>(T model, IValidator<T> validator)
    {
        return validator.ValidateAsync(model, options => options.ThrowOnFailures());
    }
}
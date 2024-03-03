using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models.Dtos;
using PersonDetection.Backend.Application.Common.Models.Requests.Login;
using PersonDetection.Backend.Application.Common.Models.Requests.Register;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class AuthorizationService(
    UserManager<IdentityUser> _userManager,
    SignInManager<IdentityUser> _signInManager) : IAuthorizationService
{
    public async Task<IResult> RegisterAsync(RegisterRequest registerRequest, IValidator<RegisterRequest> validator)
    {
        await ValidateModelAsync(registerRequest, validator);

        var user = new IdentityUser(registerRequest.UserName)
        {
            Email = registerRequest.Email
        };
        
        var createUserResult = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!createUserResult.Succeeded)
        {
            throw new InvalidCredentialsException();
        }
        
        await _signInManager.SignInAsync(user, true);
        
        return Results.Ok(UserDto.FromIdentityUser(user));
    }

    public async Task<IResult> LoginAsync(LoginRequest loginRequest, IValidator<LoginRequest> validator)
    {
        await ValidateModelAsync(loginRequest, validator);
     
        var result = await _signInManager.PasswordSignInAsync(
            loginRequest.UserName, 
            loginRequest.Password, 
            true, 
            false);
        
        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }
        
        var user = await _userManager.FindByNameAsync(loginRequest.UserName);
        
        return Results.Ok(UserDto.FromIdentityUser(user!));
    }

    public async Task<IResult> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        
        return Results.Ok();
    }

    private static Task<ValidationResult> ValidateModelAsync<T>(T model, IValidator<T> validator)
    {
        return validator.ValidateAsync(model, options => options.ThrowOnFailures());
    }
}
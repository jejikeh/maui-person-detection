using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Application.Common.Exceptions;
using PersonDetection.Backend.Application.Common.Models.Requests;
using PersonDetection.Backend.Application.Common.Models.Responses;
using PersonDetection.Backend.Infrastructure.Models.Identity;

namespace PersonDetection.Backend.Application.Services.Authorization.Implementations;

public class AuthorizationService(UserManager<User> userManager, ILoginTokensProvider loginTokensProvider)
{
    public async Task<LoginTokens> RegisterAsync(RegisterRequest registerRequest, IValidator<RegisterRequest> validator)
    {
        await validator.ValidateAsync(registerRequest, options => options.ThrowOnFailures());

        var user = new User(registerRequest.UserName, registerRequest.Email);
        var createUserResult = await userManager.CreateAsync(user, registerRequest.Password);

        if (!createUserResult.Succeeded)
        {
            throw new InvalidCredentialsException();
        }
        
        return await loginTokensProvider.GenerateLoginTokensAsync(user);
    }
}
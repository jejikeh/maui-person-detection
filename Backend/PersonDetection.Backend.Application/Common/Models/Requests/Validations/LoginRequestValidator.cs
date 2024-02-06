using FluentValidation;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Application.Common.Options;

namespace PersonDetection.Backend.Application.Common.Models.Requests.Validations;

public class LoginRequestValidator : AbstractValidator<RegisterRequest>
{
    public LoginRequestValidator(IOptions<AuthorizationModelOptions> options)
    {
        RuleFor(loginRequest => loginRequest.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address");
        
        RuleFor(loginRequest => loginRequest.Password)
            .NotEmpty()
            .MinimumLength(options.Value.MinimalPasswordLength)
            .WithMessage("Invalid password");
    }
}
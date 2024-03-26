using FluentValidation;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Infrastructure.Common.Options;

namespace PersonDetection.Backend.Application.Common.Models.Requests.Register;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IOptions<IdentityModelOptions> options)
    {
        RuleFor(registerRequest => registerRequest.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Nickname must not exceed 50 characters");
        
        RuleFor(registerRequest => registerRequest.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(registerRequest => registerRequest.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(options.Value.Password.RequiredLength)
            .WithMessage($"Password must be at least {options.Value.Password.RequiredLength} characters long");
    }
}
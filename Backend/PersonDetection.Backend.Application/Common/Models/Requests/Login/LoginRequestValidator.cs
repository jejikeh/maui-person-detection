using FluentValidation;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Infrastructure.Common.Options;

namespace PersonDetection.Backend.Application.Common.Models.Requests.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(IOptions<IdentityModelOptions> options)
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(options.Value.Password.RequiredLength)
            .WithMessage($"Password must be at least {options.Value.Password.RequiredLength} characters long");
    }
}
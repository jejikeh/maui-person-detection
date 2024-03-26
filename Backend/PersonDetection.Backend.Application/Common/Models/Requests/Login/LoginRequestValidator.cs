using FluentValidation;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Infrastructure.Common.Options;

namespace PersonDetection.Backend.Application.Common.Models.Requests.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(IOptions<IdentityModelOptions> options)
    {
        RuleFor(loginRequest => loginRequest.UserName)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.Username.IsRequired)
            .MaximumLength(options.Value.MaxUserNameLength)
            .WithMessage(ValidationErrorMessages.Username.TooLong(options.Value.MaxUserNameLength));

        RuleFor(loginRequest => loginRequest.Password)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.Password.IsRequired)
            .MinimumLength(options.Value.Password.RequiredLength)
            .WithMessage(ValidationErrorMessages.Password.WrongLength(options.Value.Password.RequiredLength));
    }
}
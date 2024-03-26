using FluentValidation;
using Microsoft.Extensions.Options;
using PersonDetection.Backend.Infrastructure.Common.Options;

namespace PersonDetection.Backend.Application.Common.Models.Requests.Register;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IOptions<IdentityModelOptions> options)
    {
        RuleFor(registerRequest => registerRequest.UserName)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.Username.IsRequired)
            .MaximumLength(options.Value.MaxUserNameLength)
            .WithMessage(ValidationErrorMessages.Username.TooLong(options.Value.MaxUserNameLength));

        RuleFor(registerRequest => registerRequest.Email)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.Email.IsRequired)
            .EmailAddress()
            .WithMessage(ValidationErrorMessages.Email.Invalid);

        RuleFor(registerRequest => registerRequest.Password)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.Password.IsRequired)
            .MinimumLength(options.Value.Password.RequiredLength)
            .WithMessage(ValidationErrorMessages.Password.WrongLength(options.Value.Password.RequiredLength));
    }
}
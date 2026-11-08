using FluentValidation;
using UserManager.Application.Contracts.Users;

namespace UserManager.Application.Validation;

public sealed class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(user => user.Name).NotEmpty().MaximumLength(100);
        RuleFor(user => user.Email).NotEmpty().EmailAddress().MaximumLength(254);
        RuleFor(user => user.Password).NotEmpty().MinimumLength(8)
            .Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches("[^a-zA-Z0-9]");
    }
}

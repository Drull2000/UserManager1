using FluentValidation;
using UserManager.Application.Contracts.Products;

namespace UserManager.Application.Validation;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Price).GreaterThanOrEqualTo(0);
        RuleFor(request => request.Description).MaximumLength(500);
        RuleFor(request => request.Quantity).GreaterThanOrEqualTo(0);
    }
}

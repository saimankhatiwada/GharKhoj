using FluentValidation;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.CreateProperty;

internal sealed class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("User id cannot be empty");

        RuleFor(p => p.Tittle)
            .NotEmpty()
            .WithMessage("Title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Title cannot be more than 100 characters");

        RuleFor(p => p.Desciption)
            .NotEmpty()
            .WithMessage("Description cannot be empty")
            .MaximumLength(500)
            .WithMessage("Description cannot be more than 500 characters");

        RuleFor(p => p.Country)
            .NotEmpty()
            .WithMessage("Country cannot be empty");

        RuleFor(p => p.State)
            .NotEmpty()
            .WithMessage("State cannot be empty");

        RuleFor(p => p.City)
            .NotEmpty()
            .WithMessage("City cannot be empty");

        RuleFor(p => p.Street)
            .NotEmpty()
            .WithMessage("Street cannot be empty");

        RuleFor(p => p.Type)
            .InclusiveBetween(1, 7)
            .WithMessage("Type must be between 1 and 7");

        RuleFor(p => p.Currency)
            .NotEmpty()
            .WithMessage("Currency cannot be empty")
            .Must(CheckCurrencySupported)
            .WithMessage("Currency is not supported");

        RuleFor(p => p.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");
    }

    private static bool CheckCurrencySupported(string value)
    {
        return !string.IsNullOrWhiteSpace(Currency.ChechCode(value).Code);
    }
}

﻿using FluentValidation;

namespace GharKhoj.Application.Users.RenewAuthorization;

internal sealed class RenewAuthorizationCommandValidator : AbstractValidator<RenewAuthorizationCommand>
{
    public RenewAuthorizationCommandValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token cannot be empty");
    }
}

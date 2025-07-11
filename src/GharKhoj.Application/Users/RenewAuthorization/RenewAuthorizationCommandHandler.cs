using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users;

namespace GharKhoj.Application.Users.RenewAuthorization;

internal sealed class RenewAuthorizationCommandHandler : ICommandHandler<RenewAuthorizationCommand, AuthorizationToken>
{
    private readonly IJwtService _jwtService;

    public RenewAuthorizationCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<AuthorizationToken>> Handle(RenewAuthorizationCommand request, CancellationToken cancellationToken)
    {
        Result<AuthorizationToken> result = await _jwtService.RenewAuthorizationAsync(request.RefreshToken, cancellationToken);

        return result.IsSuccess
            ? result.Value
            : Result.Failure<AuthorizationToken>(UserErrors.InvalidRefreshToken);
    }
}

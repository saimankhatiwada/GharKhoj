using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Messaging;
using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users;

namespace GharKhoj.Application.Users.LoginUser;

internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AuthorizationToken>
{
    private readonly IJwtService _jwtService;
    public LogInUserCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<AuthorizationToken>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        Result<AuthorizationToken> result = await _jwtService.GetAuthorizationTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return result.IsSuccess
            ? result.Value
            : Result.Failure<AuthorizationToken>(UserErrors.InvalidCredentials);
    }
}

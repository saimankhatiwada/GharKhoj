using GharKhoj.Domain.Abstractions;

namespace GharKhoj.Application.Abstracions.Authentication;

public interface IJwtService
{
    Task<Result<AuthorizationToken>> GetAuthorizationTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthorizationToken>> RenewAuthorizationAsync(string refreshToken, CancellationToken cancellationToken = default);
}

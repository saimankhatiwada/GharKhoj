using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users;

namespace GharKhoj.Application.Abstracions.Authentication;

public interface IAuthenticationService
{
    Task<Result<string>> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<Result> DeleteUserAsync(string identityId, CancellationToken cancellationToken = default);
}

using GharKhoj.Domain.Users;

namespace GharKhoj.Infrastructure.Authorization;

internal sealed class UserRolesResponse
{
    public string UserId { get; init; }
    public List<Role> Roles { get; init; } = [];
}

using GharKhoj.Domain.Abstractions;

namespace GharKhoj.Domain.Users;

internal interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    void Add(User user);
    void Update(User user);
    void Delete(User user);
}

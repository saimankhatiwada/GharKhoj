using GharKhoj.Application.Abstracions.Repositories;
using GharKhoj.Domain.Users;

namespace GharKhoj.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public override void Add(User user)
    {
        foreach (Role role in user.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(user);
    }
}

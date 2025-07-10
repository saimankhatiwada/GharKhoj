using GharKhoj.Domain.Abstractions;
using GharKhoj.Domain.Users.Events;

namespace GharKhoj.Domain.Users;

public sealed class User : Entity<UserId>
{
    private readonly List<Role> _roles = [];

    private User(UserId id, FullName fullNme, Email email) : base(id)
    {
        FullName = fullNme;
        Email = email;
    }

    private User() { }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;
    public IReadOnlyCollection<Role> Roles => [.. _roles];
    public static User RegisterUser(FullName fullName, Email email, Role role)
    {
        var user = new User(UserId.New(), fullName, email);

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        user._roles.Add(role);

        return user;
    }

    public void UpdateUser(FullName fullName, Email email)
    {
        FullName = fullName;
        Email = email;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}

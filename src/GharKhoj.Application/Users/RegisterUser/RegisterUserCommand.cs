using GharKhoj.Application.Abstracions.Messaging;

namespace GharKhoj.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string Role) : ICommand<string>;

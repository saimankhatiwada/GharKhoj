using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Messaging;

namespace GharKhoj.Application.Users.LoginUser;

public sealed record LogInUserCommand(string Email, string Password) : ICommand<AuthorizationToken>;

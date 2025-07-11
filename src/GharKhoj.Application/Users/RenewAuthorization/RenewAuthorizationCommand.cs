using GharKhoj.Application.Abstracions.Authentication;
using GharKhoj.Application.Abstracions.Messaging;

namespace GharKhoj.Application.Users.RenewAuthorization;

public sealed record RenewAuthorizationCommand(string RefreshToken) : ICommand<AuthorizationToken>;

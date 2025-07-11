using System.Text.Json.Serialization;

namespace GharKhoj.Application.Abstracions.Authentication;

public sealed class AuthorizationToken
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}

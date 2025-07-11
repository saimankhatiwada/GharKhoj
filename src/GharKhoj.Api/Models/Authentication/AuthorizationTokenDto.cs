namespace GharKhoj.Api.Models.Authentication;

public class AuthorizationTokenDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}

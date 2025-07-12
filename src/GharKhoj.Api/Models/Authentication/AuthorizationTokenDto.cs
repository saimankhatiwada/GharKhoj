namespace GharKhoj.Api.Models.Authentication;

/// <summary>
/// Represents an authorization token pair containing access and refresh tokens.
/// </summary>
public class AuthorizationTokenDto
{
    /// <summary>
    /// Gets the JWT access token used for authentication.
    /// </summary>
    public string AccessToken { get; init; }

    /// <summary>
    /// Gets the refresh token used to obtain a new access token.
    /// </summary>
    public string RefreshToken { get; init; }
}

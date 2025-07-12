namespace GharKhoj.Api.Models.Authentication;

/// <summary>
/// Data Transfer Object for refreshing authentication tokens.
/// </summary>
public sealed record RefreshTokenDto
{
    /// <summary>
    /// The refresh token used to obtain a new access token.
    /// </summary>
    public required string RefreshToken { get; init; }
}

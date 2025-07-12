namespace GharKhoj.Api.Models.Authentication;

/// <summary>
/// Data Transfer Object for user login.
/// </summary>
public sealed record LoginUserDto
{
    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Gets the password of the user.
    /// </summary>
    public required string Password { get; init; }
}

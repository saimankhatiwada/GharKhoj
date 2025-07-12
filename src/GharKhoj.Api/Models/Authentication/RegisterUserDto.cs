namespace GharKhoj.Api.Models.Authentication;

/// <summary>
/// Data Transfer Object for registering a new user.
/// </summary>
public sealed record RegisterUserDto
{
    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// Gets the password for the user account.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Gets the role assigned to the user.
    /// <c>Broker, Seeker</c>
    /// </summary>
    public required string Role { get; init; }
}

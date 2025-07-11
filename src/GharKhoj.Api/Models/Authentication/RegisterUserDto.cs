namespace GharKhoj.Api.Models.Authentication;

public sealed record RegisterUserDto
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
}

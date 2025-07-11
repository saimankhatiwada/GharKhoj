namespace GharKhoj.Api.Models.Authentication;

public sealed record LoginUserDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

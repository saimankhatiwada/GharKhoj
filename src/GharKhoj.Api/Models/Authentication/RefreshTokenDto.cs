namespace GharKhoj.Api.Models.Authentication;

public sealed record RefreshTokenDto
{
    public required string RefreshToken { get; init; }
}

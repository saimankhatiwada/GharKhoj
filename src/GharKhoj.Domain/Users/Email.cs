namespace GharKhoj.Domain.Users;

public sealed record Email(string Value)
{
    public static implicit operator string(Email email) => email.Value;
}

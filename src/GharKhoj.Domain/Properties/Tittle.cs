namespace GharKhoj.Domain.Properties;

public sealed record Tittle(string Value)
{
    public static implicit operator string(Tittle tittle) => tittle.Value;
}

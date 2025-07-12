namespace GharKhoj.Domain.Properties;

public sealed record Description(string Value)
{
    public static implicit operator string(Description description) => description.Value;
}

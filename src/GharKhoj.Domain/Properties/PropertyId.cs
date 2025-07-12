namespace GharKhoj.Domain.Properties;

public record PropertyId(string Value)
{
    public static PropertyId New() => new($"p_{Ulid.NewUlid()}");
}

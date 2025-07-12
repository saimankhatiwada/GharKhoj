namespace GharKhoj.Domain.Properties;

public sealed record Currency
{
    internal static readonly Currency None = new(string.Empty);
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Npr = new("NPR");

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ??
            throw new ApplicationException("The currency code is invalid");
    }

    public static Currency ChechCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ?? None;
    }

    public static readonly IReadOnlyCollection<Currency> All =
    [
        None,
        Usd,
        Npr
    ];
}

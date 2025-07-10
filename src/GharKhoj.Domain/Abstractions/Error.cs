namespace GharKhoj.Domain.Abstractions;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided");

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; }

    public string Description { get; }
}

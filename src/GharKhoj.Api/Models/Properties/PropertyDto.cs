namespace GharKhoj.Api.Models.Properties;

public sealed record PropertyDto
{
    public string Id { get; init; }
    public string Tittle { get; init; }
    public string Description { get; init; }
    public LocationDto Location { get; init; }
    public int Type { get; init; }
    public MoneyDto Price { get; init; }
}

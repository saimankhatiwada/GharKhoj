namespace GharKhoj.Api.Models.Properties;

/// <summary>
/// Represents a property with details such as title, description, location, type, and price.
/// </summary>
public sealed record PropertyDto
{
    /// <summary>
    /// Gets the unique identifier of the property.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the title of the property.
    /// </summary>
    public string Tittle { get; init; }

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Gets the location details of the property.
    /// </summary>
    public LocationDto Location { get; init; }

    /// <summary>
    /// Gets the type of the property as an integer.
    /// </summary>
    public int Type { get; init; }

    /// <summary>
    /// Gets the price details of the property.
    /// </summary>
    public MoneyDto Price { get; init; }
}

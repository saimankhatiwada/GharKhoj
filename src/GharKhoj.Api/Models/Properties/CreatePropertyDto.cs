using Newtonsoft.Json;

namespace GharKhoj.Api.Models.Properties;

/// <summary>
/// Data Transfer Object for creating a new property.
/// </summary>
/// <param name="Tittle">The title of the property.</param>
/// <param name="Description">A detailed description of the property.</param>
/// <param name="Location">The location details of the property.</param>
/// <param name="Type">The type identifier of the property.</param>
/// <param name="Price">The price details of the property.</param>
public sealed record CreatePropertyDto(
    string Tittle,
    string Description,
    LocationDto Location,
    [property: JsonRequired] int Type,
    MoneyDto Price);

namespace GharKhoj.Api.Models.Properties;

/// <summary>
/// Represents a location with country, state, city, and street information.
/// </summary>
/// <param name="Country">The country of the location.</param>
/// <param name="State">The state or province of the location.</param>
/// <param name="City">The city of the location.</param>
/// <param name="Street">The street address of the location.</param>
public sealed record LocationDto(string Country, string State, string City, string Street);

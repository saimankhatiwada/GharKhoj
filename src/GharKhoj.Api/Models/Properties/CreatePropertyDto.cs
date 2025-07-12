using Newtonsoft.Json;

namespace GharKhoj.Api.Models.Properties;

public sealed record CreatePropertyDto(
    string Tittle, 
    string Description, 
    LocationDto Location, 
    [property: JsonRequired] int Type, 
    MoneyDto Price);

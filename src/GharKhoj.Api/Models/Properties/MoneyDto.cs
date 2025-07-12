using Newtonsoft.Json;

namespace GharKhoj.Api.Models.Properties;

public sealed record MoneyDto([property: JsonRequired] decimal Amount, string Currency);

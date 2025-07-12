using Newtonsoft.Json;

namespace GharKhoj.Api.Models.Properties;

/// <summary>
/// Represents a monetary value with an amount and a currency code.
/// </summary>
/// <param name="Amount">The numeric value of the money.</param>
/// <param name="Currency">The ISO currency code (e.g., "USD", "NPR").</param>
public sealed record MoneyDto([property: JsonRequired] decimal Amount, string Currency);

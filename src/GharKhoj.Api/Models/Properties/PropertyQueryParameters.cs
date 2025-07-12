using GharKhoj.Api.Models.Common;

namespace GharKhoj.Api.Models.Properties;

public sealed record PropertyQueryParameters : AcceptHeaderDto
{
    /// <summary>
    /// A comma-separated list of fields to include in the response for data shaping.
    /// Use this parameter to request only the necessary fields.
    /// </summary>
    public string? Fields { get; init; }
}

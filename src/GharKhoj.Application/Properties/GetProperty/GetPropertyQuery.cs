using GharKhoj.Application.Abstracions.Caching;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.GetProperty;

public sealed record GetPropertyQuery(string PropertyId, string? Fields) : ICachedQuery<Property>
{
    public string CacheKey => $"{nameof(GetPropertyQuery)}-{PropertyId}-{Fields}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}

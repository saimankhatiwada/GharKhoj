using GharKhoj.Application.Abstracions.Caching;
using GharKhoj.Application.Common;
using GharKhoj.Domain.Properties;

namespace GharKhoj.Application.Properties.GetProperties;

public sealed record GetPropertiesQuery(string? Search, string? Sort, string? Fields, int? Page, int? PageSize) 
    : ICachedQuery<PaginationResult<Property>>
{
    public string CacheKey => $"{nameof(GetPropertiesQuery)}-{Search}-{Sort}-{Fields}-{Page}-{PageSize}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}

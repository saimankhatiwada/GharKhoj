using GharKhoj.Application.Abstracions.Messaging;

namespace GharKhoj.Application.Abstracions.Caching;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;
public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}

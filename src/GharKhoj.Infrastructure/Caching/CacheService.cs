using System.Buffers;
using System.Text;
using GharKhoj.Application.Abstracions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace GharKhoj.Infrastructure.Caching;

internal sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await _cache.GetAsync(key, cancellationToken);

        return bytes is null ? default : Deserialize<T>(bytes);
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);

        return _cache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
        _cache.RemoveAsync(key, cancellationToken);

    private static T Deserialize<T>(byte[] bytes)
    {
        string json = Encoding.UTF8.GetString(bytes);

        return JsonConvert.DeserializeObject<T>(json)!;
    }

    private static byte[] Serialize<T>(T value)
    {
        string json = JsonConvert.SerializeObject(value);

        return Encoding.UTF8.GetBytes(json);
    }
}

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetDataAsync<T>(this IDistributedCache cache, string cacheKey)
    {
        var data = await cache.GetStringAsync(cacheKey);
        if (data != null)
        {
            var cachedData = JsonSerializer.Deserialize<T>(data);
            return cachedData;
        }

        return default(T);
    }

    public static async Task SetDataAsync<T>(
        this IDistributedCache cache,
        string cacheKey,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null
    )
    {
        var data = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions();

        if (slidingExpiration.HasValue)
        {
            options.SetSlidingExpiration(slidingExpiration.Value);
        }

        if (absoluteExpiration.HasValue)
        {
            options.SetAbsoluteExpiration(absoluteExpiration.Value);
        }
        await cache.SetStringAsync(cacheKey, data, options);
    }

    public static async Task<T?> GetOrCreateAsync<T>(
        this IDistributedCache cache,
        string cacheKey,
        Func<Task<T>> createFunc,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null
    )
    {
        var cachedData = await cache.GetDataAsync<T>(cacheKey);
        if (!EqualityComparer<T>.Default.Equals(cachedData, default(T)))
        {
            return cachedData;
        }

        var newData = await createFunc();
        await cache.SetDataAsync(cacheKey, newData, absoluteExpiration, slidingExpiration);
        return newData;
    }
}

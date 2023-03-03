using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Redis_Cache;

public class CacheProvider : ICacheprovider
{
    private readonly IDistributedCache _cache;

    public CacheProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetAsync<T>(string key) where T : class
    {
        var data = await _cache.GetStringAsync(key);
        return data == null ? null : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan time) where T : class
    {
        DistributedCacheEntryOptions options  = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(time);

        var data = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, data, options);
    }

    public async Task ClearAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }


    public bool TryGetValue<T>(string key, out T result)
    {
        var json = _cache.GetString(key);
        if (json == null)
        {
            result = default(T);
            return false;
        }

        result = JsonSerializer.Deserialize<T>(json);
        return true;
    }

}
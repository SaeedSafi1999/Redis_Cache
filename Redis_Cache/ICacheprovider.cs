namespace Redis_Cache;

public interface ICacheprovider
{
    Task<T> GetAsync<T>(string key) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan time) where T : class;
    Task ClearAsync(string key);
    bool TryGetValue<T>(string key, out T result);
}
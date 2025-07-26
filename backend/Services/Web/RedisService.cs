using StackExchange.Redis;

using Newtonsoft.Json;

public class RedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    /// <summary>
    /// 设置缓存对象（默认过期时间可选）
    /// </summary>
    public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonConvert.SerializeObject(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    /// <summary>
    /// 获取缓存对象
    /// </summary>
    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue ? JsonConvert.DeserializeObject<T>(value!) : default;
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    public async Task<bool> DeleteAsync(string key)
    {
        return await _db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// 判断是否存在 key
    /// </summary>
    public async Task<bool> ExistsAsync(string key)
    {
        return await _db.KeyExistsAsync(key);
    }

    /// <summary>
    /// 设置过期时间
    /// </summary>
    public async Task<bool> ExpireAsync(string key, TimeSpan expiry)
    {
        return await _db.KeyExpireAsync(key, expiry);
    }

    /// <summary>
    /// 获取剩余时间
    /// </summary>
    public async Task<TimeSpan?> GetTtlAsync(string key)
    {
        return await _db.KeyTimeToLiveAsync(key);
    }
}

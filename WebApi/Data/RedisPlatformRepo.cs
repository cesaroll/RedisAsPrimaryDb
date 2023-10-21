/*
 * @author: Cesar Lopez
 * @copyright 2023 - All rights reserved
 */

using System.Data;
using System.Text.Json;
using StackExchange.Redis;
using WebApi.Models;

namespace WebApi.Data;

public class RedisPlatformRepo : IPlatformRepo
{
    private const string PlatformKey = "Platforms";
    private readonly IConnectionMultiplexer _redisMultiplexer;

    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redisMultiplexer = redis;
    }

    private IDatabase RedisDb => _redisMultiplexer.GetDatabase();

    public async Task<Platform> CreatePlatformAsync(Platform platform)
    {
        var serializedPlatform = JsonSerializer.Serialize(platform);

        await RedisDb.HashSetAsync(PlatformKey, platform.Id, serializedPlatform);

        return platform;
    }

    public async Task<Platform?> GetPlatformByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var serializedPlatform = await RedisDb.HashGetAsync(PlatformKey, id);

        if (serializedPlatform.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<Platform>(serializedPlatform.ToString());
    }

    public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        var hashEntries = await RedisDb.HashGetAllAsync(PlatformKey);

        if (hashEntries.Length == 0)
            return new List<Platform>().AsEnumerable();

        var platforms = hashEntries.Select(h => h.Value).Where(v => !v.IsNullOrEmpty);

        return platforms
            .Select(p => JsonSerializer.Deserialize<Platform>(p)!)
            .ToList();
    }

    public async Task DeletePlatform(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        await RedisDb.HashDeleteAsync(PlatformKey, id);
    }
}

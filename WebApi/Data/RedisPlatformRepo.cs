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
    private readonly IConnectionMultiplexer _redisMultiplexer;

    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redisMultiplexer = redis;
    }

    private IDatabase RedisDb => _redisMultiplexer.GetDatabase();

    public async Task<Platform> CreatePlatformAsync(Platform platform)
    {
        var serializedPlatform = JsonSerializer.Serialize(platform);

        await RedisDb.StringSetAsync(platform.Id, serializedPlatform);

        return platform;
    }

    public async Task<Platform?> GetPlatformByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var serializedPlatform = await RedisDb.StringGetAsync(id);

        if (serializedPlatform.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<Platform>(serializedPlatform.ToString());
    }

    public Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        throw new NotImplementedException();
    }
}

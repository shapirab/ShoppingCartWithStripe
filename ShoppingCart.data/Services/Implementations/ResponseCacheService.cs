using ShoppingCart.data.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Implementations
{
    public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
    {
        private readonly IDatabase db = redis.GetDatabase(1);

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string? serializedResponse = JsonSerializer.Serialize(response, options);

            await db.StringSetAsync(cacheKey, serializedResponse, timeToLive);

        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            RedisValue cachedResponse = await db.StringGetAsync(cacheKey);
            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }
            return cachedResponse;
        }

        public async Task RemoveChacheByPatternAsync(string pattern)
        {
            IServer? server = redis.GetServer(redis.GetEndPoints().First());
            RedisKey[]? keys = server.Keys(database: 1, pattern: $"*{pattern}*").ToArray();

            if(keys.Length != 0)
            {
                await db.KeyDeleteAsync(keys);
            }
        }
    }
}

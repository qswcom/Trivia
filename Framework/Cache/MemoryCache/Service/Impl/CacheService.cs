using System;
using System.Threading.Tasks;
using Com.Qsw.Framework.Cache.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Com.Qsw.Framework.Cache.MemoryCache
{
    public class CacheService : ICacheService
    {
        private readonly ILogger logger;

        private readonly Microsoft.Extensions.Caching.Memory.MemoryCache cache =
            new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());

        public CacheService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<CacheService>();
        }

        public async Task<T> Set<T>(string key, T value, DateTimeOffset? absoluteExpiration)
        {
            await Task.CompletedTask;
            logger.LogDebug($"Set {key} to cache service.");

            if (key == null)
            {
                return default;
            }

            if (value == null)
            {
                return default;
            }

            if (absoluteExpiration == null)
            {
                absoluteExpiration = DateTimeOffset.MaxValue;
            }

            cache.Set(key, value, absoluteExpiration.Value);
            return value;
        }

        public async Task<T> Get<T>(string key)
        {
            await Task.CompletedTask;
            logger.LogDebug($"Get {key} from cache service.");

            if (key == null)
            {
                return default;
            }

            object obj = cache.Get(key);

            if (!(obj is T result))
            {
                result = default;
            }

            return result;
        }

        public async Task Delete(string key)
        {
            await Task.CompletedTask;
            logger.LogDebug($"Delete {key} from cache service.");

            if (key == null)
            {
                return;
            }

            cache.Remove(key);
        }
    }
}
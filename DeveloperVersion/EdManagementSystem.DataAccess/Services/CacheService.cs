using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace EdManagementSystem.DataAccess.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemFunc, TimeSpan expirationTime)
        {
            if (!_memoryCache.TryGetValue(key, out T item))
            {
                item = await getItemFunc();

                if (item != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(expirationTime);

                    _memoryCache.Set(key, item, cacheEntryOptions);
                }
            }

            return item;
        }

        public bool InvalidateCache(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}

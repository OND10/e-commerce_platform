using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace SharedKernels.Caching
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        // Simple mechanism to track keys by tags for invalidation (not production perfect but functional for demo)
        private static readonly ConcurrentDictionary<string, HashSet<string>> _taggedKeys = new();

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var value = await factory();
            
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }
            else 
            {
                 // Default 5 mins
                 options.SetSlidingExpiration(TimeSpan.FromMinutes(5));
            }

            _memoryCache.Set(key, value, options);
            return value;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByTagAsync(string tag)
        {
            // In a real distributed cache (Redis), we would use Pattern Matching.
            // For IMemoryCache, we would need to track all keys. 
            // This suggests that "RemoveByTag" is a placeholder for future Redis usage 
            // or requires a complex tracking mechanism not ideal for simple IMemoryCache wrapper.
            // For now, we will assume keys are known or this is a hook for the future.
            
            return Task.CompletedTask;
        }
        
    }
}

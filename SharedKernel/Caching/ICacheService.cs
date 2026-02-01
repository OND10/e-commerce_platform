using System;
using System.Threading.Tasks;

namespace SharedKernel.Caching
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByTagAsync(string tag);
    }
}

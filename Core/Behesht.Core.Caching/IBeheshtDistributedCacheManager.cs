using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Caching
{
    public interface IBeheshtDistributedCacheManager : IBeheshtCacheManager
    {
        Task<TItem> SetAsync<TItem>(string key, TItem value);
        Task<TItem> SetAsync<TItem>(Enum memoryCacheKeyEnum, string key, TItem value);
        Task<TItem> GetAsync<TItem>(string key);
        Task<TItem> GetAsync<TItem>(Enum memoryCacheKeyEnum, string key);
        Task<TItem> GetOrCreateAsync<TItem>(string key, Func<TItem> factory);
        Task<TItem> GetOrCreateAsync<TItem>(Enum memoryCacheKeyEnum, string key, Func<TItem> factory);
        Task<bool> RemoveAsync(Enum key);
        Task<bool> RemoveAsync(params Enum[] keys);
        Task<bool> RemoveAsync(string key);
        Task<bool> RemoveAsync(Enum memoryCacheKeyEnum, string key);
        Task ClearAsync();
    }
}

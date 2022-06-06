using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Caching
{
    public interface IBeheshtCacheManager
    {
        TItem Set<TItem>(string key, TItem value);
        TItem Set<TItem>(Enum memoryCacheKeyEnum, string key, TItem value);
        TItem Get<TItem>(string key);
        TItem Get<TItem>(Enum memoryCacheKeyEnum, string key);
        bool TryGetValue<TItem>(string key, out TItem value);
        bool TryGetValue<TItem>(Enum memoryCacheKeyEnum, string key, out TItem value);
        TItem GetOrCreate<TItem>(string key, Func<TItem> factory);
        TItem GetOrCreate<TItem>(Enum memoryCacheKeyEnum, string key, Func<TItem> factory);
        bool Remove(Enum key);
        bool Remove(params Enum[] keys);
        bool Remove(string key);
        bool Remove(Enum memoryCacheKeyEnum, string key);
        void Clear();
        string[] GetCacheKeys();
        bool RemoveByPattern(string pattern);
    }
}

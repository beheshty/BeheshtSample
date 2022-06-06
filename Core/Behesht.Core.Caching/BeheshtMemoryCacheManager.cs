using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Behesht.Core.Caching
{
    public class BeheshtMemoryCacheManager : IBeheshtCacheManager
    {

        private static readonly List<string> _cacheKeys = new List<string>();
        private static MemoryCache _cache;
        private MemoryCacheEntryOptions _defaultOptions;

        public BeheshtMemoryCacheManager()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        private MemoryCacheEntryOptions DefaultMemoryCacheEntryOptions
        {
            get
            {
                if (_defaultOptions == null)
                {
                    _defaultOptions = new MemoryCacheEntryOptions
                    {
                        
                        AbsoluteExpirationRelativeToNow = new TimeSpan(24, 0, 0)
                    };
                    _defaultOptions.RegisterPostEvictionCallback(PostEviction);
                }
                return _defaultOptions;
            }
        }

        public void Clear()
        {
            foreach (var key in _cacheKeys.Where(p => p != null).ToList())
            {
                Remove(key);
            }
            _cacheKeys.Clear();
            _cache.Dispose();
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public IList<string> GetCacheKeys()
        {
            return _cacheKeys.ToList();
        }


        private static void AddKey(string key)
        {
            lock (string.Intern(key))
            {
                if (!_cacheKeys.Contains(key.ToString()))
                {
                    _cacheKeys.Add(key);
                }
            }
        }

        private static void RemoveKey(string key)
        {
            if (_cacheKeys.Contains(key))
            {
                lock (string.Intern(key))
                {
                    _cacheKeys.Remove(key);
                }
            }
        }

        private static string GenerateCacheKey(Enum memoryCacheKeyEnum, string key)
        {
            return memoryCacheKeyEnum.ToString() + "_" + key;
        }

        public TItem GetOrCreate<TItem>(string key, Func<TItem> factory)
        {
            lock (string.Intern(key))
            {
                return _cache.GetOrCreate(key.ToString(), entry =>
                {
                    entry.SetOptions(DefaultMemoryCacheEntryOptions);
                    AddKey(key);
                    return factory.Invoke();
                });
            }
        }

        public TItem GetOrCreate<TItem>(Enum memoryCacheKeyEnum, string key, Func<TItem> factory)
        {
            return GetOrCreate<TItem>(GenerateCacheKey(memoryCacheKeyEnum, key), factory);
        }

        public bool Remove(Enum key)
        {
            foreach (var item in _cacheKeys.Where(p => p.ToString().StartsWith(key.ToString() + "_")).ToList())
            {
                try
                {
                    Remove(item);
                }
                catch (Exception)
                { }
            }
            return true;
        }

        public bool Remove(params Enum[] keys)
        {
            foreach (var item in keys)
            {
                Remove(item);
            }
            return true;
        }

        public bool Remove(string key)
        {
            lock (string.Intern(key))
            {
                try
                {
                    _cache.Remove(key);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public bool Remove(Enum memoryCacheKeyEnum, string key)
        {
            var strKey = GenerateCacheKey(memoryCacheKeyEnum, key);
            lock (string.Intern(strKey))
            {
                try
                {
                    _cache.Remove(strKey);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public TItem Set<TItem>(string key, TItem value)
        {
            lock (string.Intern(key))
            {
                AddKey(key);
                return _cache.Set(key, value, DefaultMemoryCacheEntryOptions);
            }
        }

        public TItem Set<TItem>(Enum memoryCacheKeyEnum, string key, TItem value)
        {
            return Set(GenerateCacheKey(memoryCacheKeyEnum, key), value);
        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {
            lock (string.Intern(key))
            {
                return _cache.TryGetValue(key, out value);
            }
        }

        public bool TryGetValue<TItem>(Enum memoryCacheKeyEnum, string key, out TItem value)
        {
            return TryGetValue<TItem>(GenerateCacheKey(memoryCacheKeyEnum, key), out value);
        }

        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            if (reason == EvictionReason.Replaced)
                return;
            if (key != null)
                RemoveKey(key.ToString());
        }


        string[] IBeheshtCacheManager.GetCacheKeys()
        {
            throw new NotImplementedException();
        }

        public TItem Get<TItem>(string key)
        {
            lock (string.Intern(key))
            {
                return _cache.Get<TItem>(key);
            }
        }

        public TItem Get<TItem>(Enum memoryCacheKeyEnum, string key)
        {
            var strKey = GenerateCacheKey(memoryCacheKeyEnum, key);
            lock (string.Intern(strKey))
            {
                return Get<TItem>(strKey);
            }
        }

        public bool RemoveByPattern(string pattern)
        {
            pattern = pattern.TrimEnd('*');
            var cacheKeys = _cacheKeys.Where(p=>p.StartsWith(pattern)).ToArray();
            foreach (var cacheKey in cacheKeys)
            {
                Remove(cacheKey);
            }
            return true;
        }
    }
}

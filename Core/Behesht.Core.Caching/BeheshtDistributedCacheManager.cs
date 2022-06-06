using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Behesht.Core.Extensions;
using Behesht.Core.Caching.Models;
using StackExchange.Redis;

namespace Behesht.Core.Caching
{
    public class BeheshtDistributedCacheManager : IBeheshtDistributedCacheManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly string _cacheKeyPrefix;
        private readonly string _host;

        public BeheshtDistributedCacheManager(IDistributedCache distributedCache, IConnectionMultiplexer multiplexer, DistributedCacheConfigs options)
        {
            _distributedCache = distributedCache;
            _multiplexer = multiplexer;
            _cacheKeyPrefix = options.KeyPrefix;
            _host = $"{options.Endpoint}:{options.Port}";
        }

        //private void AddKey(string key)
        //{
        //    var keys = InternalGetOrCreate(_allCacheKey, () =>
        //    {
        //        return new string[] { key };
        //    });
        //    bool keysChaned = false;
        //    if (!keys.Contains(key))
        //    {
        //        Array.Resize(ref keys, keys.Length + 1);
        //        //***length - 1 = ^1
        //        keys[^1] = key;
        //        keysChaned = true;
        //    }
        //    if (keysChaned)
        //    {
        //        InternalSet(_allCacheKey, keys);
        //    }
        //}


        //private void RemoveKey(string key)
        //{
        //    if (InternalTryGetValue(_allCacheKey, out string[] keys))
        //    {
        //        if (keys.Contains(key))
        //        {
        //            keys = Array.FindAll(keys, k => k != key).ToArray();
        //            InternalSet(_allCacheKey, keys);
        //        }
        //    }
        //}

        private string GenerateCacheKey(string key, Enum memoryCacheKeyEnum = null)
        {
            string resultKey = $"{_cacheKeyPrefix}_";
            if (memoryCacheKeyEnum != null)
            {
                resultKey += $"{memoryCacheKeyEnum}_";
            }
            resultKey += key;
            return resultKey;
        }

        public void Clear()
        {
            RemoveByPattern(_cacheKeyPrefix + "*");
            //var keys = GetCacheKeys();
            //foreach (var key in keys)
            //{
            //    Remove(key);
            //}
            //_cacheKeys.Clear();
        }

        /// <summary>
        /// returns only local keys, not distributed ones
        /// </summary>
        /// <returns></returns>
        public string[] GetCacheKeys()
        {
            IEnumerable<RedisKey> keys = GetKeysByPattern(_cacheKeyPrefix + "*");
            return keys.Select(p => p.ToString()).ToArray();
        }

        private IEnumerable<RedisKey> GetKeysByPattern(string pattern)
        {
            return _multiplexer.GetServer(_host).Keys(pattern: pattern);
        }

        private IAsyncEnumerable<RedisKey> GetKeysByPatternAsync(string pattern)
        {
            return _multiplexer.GetServer(_host).KeysAsync(pattern: pattern);
        }

        public TItem GetOrCreate<TItem>(string key, Func<TItem> factory)
        {
            string keyStr = GenerateCacheKey(key);
            return InternalGetOrCreate(keyStr, factory);
        }

        private TItem InternalGetOrCreate<TItem>(string keyStr, Func<TItem> factory)
        {
            lock (string.Intern(keyStr))
            {
                TItem result;
                var cachedBytes = _distributedCache.Get(keyStr);
                if (cachedBytes == null)
                {
                    result = factory.Invoke();
                    _distributedCache.Set(keyStr, JsonSerializer.SerializeToUtf8Bytes(result));
                }
                else
                {
                    result = JsonSerializer.Deserialize<TItem>(cachedBytes);
                }
                return result;
            }
        }

        public TItem GetOrCreate<TItem>(Enum memoryCacheKeyEnum, string key, Func<TItem> factory)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return InternalGetOrCreate(keyStr, factory);
        }


        public TItem Set<TItem>(string key, TItem value)
        {
            string keyStr = GenerateCacheKey(key);
            return InternalSet(keyStr, value);
        }

        private TItem InternalSet<TItem>(string keyStr, TItem value)
        {
            lock (string.Intern(keyStr))
            {
                _distributedCache.Set(keyStr, JsonSerializer.SerializeToUtf8Bytes(value));
                return value;
            }
        }

        public TItem Set<TItem>(Enum memoryCacheKeyEnum, string key, TItem value)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return InternalSet(keyStr, value);
        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {
            var keyStr = GenerateCacheKey(key);
            return InternalTryGetValue(keyStr, out value);
        }

        private bool InternalTryGetValue<TItem>(string keyStr, out TItem value)
        {
            lock (string.Intern(keyStr))
            {
                var cachedBytes = _distributedCache.Get(keyStr);
                if (cachedBytes != null)
                {
                    value = JsonSerializer.Deserialize<TItem>(cachedBytes);
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
        }

        public bool TryGetValue<TItem>(Enum memoryCacheKeyEnum, string key, out TItem value)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return InternalTryGetValue(keyStr, out value);
        }

        public async Task<TItem> SetAsync<TItem>(string key, TItem value)
        {
            var keyStr = GenerateCacheKey(key);
            return await InternalSetAsync(keyStr, value);
        }

        private async Task<TItem> InternalSetAsync<TItem>(string keyStr, TItem value)
        {
            await _distributedCache.SetAsync(keyStr, JsonSerializer.SerializeToUtf8Bytes(value));
            return value;
        }

        public async Task<TItem> SetAsync<TItem>(Enum memoryCacheKeyEnum, string key, TItem value)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return await InternalSetAsync(keyStr, value);
        }

        public async Task<TItem> GetAsync<TItem>(string key)
        {
            var keyStr = GenerateCacheKey(key);
            return await InternalGetAsync<TItem>(keyStr);
        }

        private async Task<TItem> InternalGetAsync<TItem>(string keyStr)
        {
            var cachedBytes = await _distributedCache.GetAsync(keyStr);
            return JsonSerializer.Deserialize<TItem>(cachedBytes);
        }

        public async Task<TItem> GetAsync<TItem>(Enum memoryCacheKeyEnum, string key)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return await InternalGetAsync<TItem>(keyStr);
        }

        public async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<TItem> factory)
        {
            var keyStr = GenerateCacheKey(key);
            return await InternalGetOrCreateAsync(keyStr, factory);
        }

        private async Task<TItem> InternalGetOrCreateAsync<TItem>(string keyStr, Func<TItem> factory)
        {
            TItem result;
            var cachedBytes = await _distributedCache.GetAsync(keyStr);
            bool dataWasCached = true;
            lock (string.Intern(keyStr))
            {
                if (cachedBytes == null)
                {
                    result = factory.Invoke();
                    dataWasCached = false;
                }
                else
                {
                    result = JsonSerializer.Deserialize<TItem>(cachedBytes);
                }
            }
            if (!dataWasCached)
            {

                await _distributedCache.SetAsync(keyStr, JsonSerializer.SerializeToUtf8Bytes(result));
            }
            return result;
        }

        public async Task<TItem> GetOrCreateAsync<TItem>(Enum memoryCacheKeyEnum, string key, Func<TItem> factory)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return await InternalGetOrCreateAsync(keyStr, factory);
        }

        public async Task<bool> RemoveAsync(Enum key)
        {
            var keys = GetKeysByPattern(GenerateCacheKey("", key) + "*");
            foreach (var item in keys)
            {
                await RemoveAsync(item);
            }
            return true;
        }

        public async Task<bool> RemoveAsync(params Enum[] keys)
        {
            foreach (var item in keys)
            {
                await RemoveAsync(item);
            }
            return true;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            string keyStr = GenerateCacheKey(key);
            bool removalAllowed = false;
            lock (string.Intern(keyStr))
            {
                var keys = GetCacheKeys();
                if (keys.Contains(keyStr))
                {
                    removalAllowed = true;
                }
            }

            if (removalAllowed)
            {
                await _distributedCache.RemoveAsync(keyStr);
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveAsync(Enum memoryCacheKeyEnum, string key)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return await RemoveAsync(keyStr);
        }

        public bool Remove(Enum key)
        {
            var keyStr = GenerateCacheKey("", key);
            return RemoveByPattern(keyStr + "*");
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
            string keyStr = GenerateCacheKey(key);
            lock (string.Intern(keyStr))
            {
                var keys = GetCacheKeys();
                if (keys.Contains(keyStr))
                {
                    _distributedCache.Remove(keyStr);
                    return true;
                }
                return false;
            }
        }

        public bool Remove(Enum memoryCacheKeyEnum, string key)
        {
            var keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return Remove(keyStr);
        }

        public bool RemoveByPattern(string pattern)
        {
            var keyStr = GenerateCacheKey(pattern);
            var keys = GetKeysByPattern(keyStr);
            foreach (var item in keys)
            {
                _distributedCache.Remove(item.ToString());
            }
            return true;
        }

        public async Task<bool> RemoveByPatternAsync(string pattern)
        {
            var keyStr = GenerateCacheKey(pattern);
            var keys = GetKeysByPatternAsync(keyStr);
            await foreach (var item in keys)
            {
                await _distributedCache.RemoveAsync(item.ToString());
            }
            return true;
        }

        public async Task ClearAsync()
        {
            //foreach (var key in _cacheKeys.Where(p => p != null).ToList())
            //{
            //    await RemoveAsync(key);
            //}
            //_cacheKeys.Clear();
            await RemoveByPatternAsync(_cacheKeyPrefix + "*");
        }

        public TItem Get<TItem>(string key)
        {
            string keyStr = GenerateCacheKey(key);
            return InternalGet<TItem>(keyStr);
        }

        private TItem InternalGet<TItem>(string keyStr)
        {
            var cachedBytes = _distributedCache.Get(keyStr);
            return JsonSerializer.Deserialize<TItem>(cachedBytes);
        }

        public TItem Get<TItem>(Enum memoryCacheKeyEnum, string key)
        {
            string keyStr = GenerateCacheKey(key, memoryCacheKeyEnum);
            return InternalGet<TItem>(keyStr);
        }
    }
}

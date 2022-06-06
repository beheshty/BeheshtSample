using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Behesht.Core;
using Behesht.Core.Caching;
using Behesht.Core.Caching.Models;
using Behesht.Core.Data;
using Behesht.Core.Models;
using Behesht.Core.Models.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Behesht.Services
{
    public class BaseCacheableService<TEntity, TModel> : BaseService<TEntity, TModel> where TEntity : BaseEntity<long> where TModel : BaseModel
    {
        private readonly IBeheshtCacheManager _BeheshtCacheManager;
        private readonly string _cacheKeyPrefix;

        public BaseCacheableService(IRepository<TEntity> repository, IMapper mapper, IServiceProvider serviceProvider, CacheEnableConfig cacheConfig) : base(repository, mapper)
        {
            if (cacheConfig.DistributedCacheEnabled)
            {
                _BeheshtCacheManager = serviceProvider.GetRequiredService<IBeheshtDistributedCacheManager>();
                _cacheKeyPrefix = typeof(TEntity).Name;
            }
            else
            {
                _BeheshtCacheManager = serviceProvider.GetRequiredService<IBeheshtCacheManager>();
                _cacheKeyPrefix = typeof(TEntity).Name;
            }
        }

        public override void Delete(long id, bool softDelete = true)
        {
            RemoveCacheByPattern();
            base.Delete(id, softDelete);
        }

        private void RemoveCacheByPattern()
        {
            _BeheshtCacheManager.RemoveByPattern(_cacheKeyPrefix + "_*");
        }

        public override void Delete(long[] ids, bool softDelete = true)
        {
            RemoveCacheByPattern();
            base.Delete(ids, softDelete);
        }

        public override PagedList<TModel> Get(PagedListInputMeta meta)
        {
            var cacheKey = $"{_cacheKeyPrefix}_GetPaged_{JsonSerializer.Serialize(meta)}";
            return _BeheshtCacheManager.GetOrCreate(cacheKey, () =>
            {
                return base.Get(meta);
            });
        }

        public override TModel GetById(long id)
        {
            var cacheKey = $"{_cacheKeyPrefix}_GetById_{id}";
            return _BeheshtCacheManager.GetOrCreate(cacheKey, () =>
            {
                return base.GetById(id);
            });
        }

        public override void Insert(IEnumerable<TModel> models)
        {
            RemoveCacheByPattern();
            base.Insert(models);
        }

        public override void Insert(TModel model)
        {
            RemoveCacheByPattern();
            base.Insert(model);
        }

        public override void Update(IEnumerable<TModel> models)
        {
            RemoveCacheByPattern();
            base.Update(models);
        }

        public override void Update(TModel model)
        {
            RemoveCacheByPattern();
            base.Update(model);
        }
    }
}

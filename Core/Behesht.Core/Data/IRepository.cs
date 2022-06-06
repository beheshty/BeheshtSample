using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behesht.Core.Data
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity<long>
    {
        TEntity FindById(long id);
        IEnumerable<TEntity> FindByIds(long[] ids);
        void Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
        IQueryable<TEntity> EntitiesTable { get; }
        IQueryable<TEntity> EntitiesTableNoTracking { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Behesht.Core.Data;
using Behesht.Core;

namespace Behesht.Data.Repositories
{
    public class EfRepository<TEntity> : IEfRepository<TEntity> where TEntity : class, IBaseEntity<long>
    {
        private readonly IDbContext _dbContext;
        private DbSet<TEntity> _entities;


        public EfRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch
            {
                _dbContext.ClearChangeTracker();
                throw;
            }
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                _dbContext.SaveChanges();
            }
            catch
            {
                _dbContext.ClearChangeTracker();
                throw;
            }
        }

        public virtual TEntity FindById(long id)
        {
            return Entities.Find(id);
        }

        public virtual IEnumerable<TEntity> FindByIds(long[] ids)
        {

            return Entities.Where(p => ids.Contains(p.Id)).ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Add(entity);
                _dbContext.SaveChanges();
            }
            catch
            {
                _dbContext.ClearChangeTracker();
                throw;
            }
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AddRange(entities);
                _dbContext.SaveChanges();
            }
            catch
            {
                _dbContext.ClearChangeTracker();
                throw;
            }
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                _dbContext.SaveChanges();
            }
            catch
            {
                _dbContext.ClearChangeTracker();
                throw;
            }
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                _dbContext.SaveChanges();
            }
            catch
            {
                _dbContext.ClearChangeTracker();
                throw;
            }
        }

        public IQueryable<TEntity> EntitiesTable => Entities;

        public IQueryable<TEntity> EntitiesTableNoTracking => Entities.AsNoTracking();

        protected DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<TEntity>();

                return _entities;
            }
        }

        public DbContext Context => _dbContext.DbContext;

    }
}

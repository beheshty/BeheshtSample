using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Behesht.Core.Attributes;
using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Behesht.Data
{
    public class BeheshtDbContext : DbContext, IDbContext
    {
        public DbContext DbContext { get => this; }

        public BeheshtDbContext(DbContextOptions options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(et => et.ClrType != null && typeof(BaseEntity).IsAssignableFrom(et.ClrType) && et.BaseType == null))
            {
                //Adding query filter to all entities that have isdelete property
                var isDeleteProp = entity.GetProperties().FirstOrDefault(p => p.Name.ToLower() == "isdelete");
                if (isDeleteProp != null)
                {
                    ParameterExpression param = Expression.Parameter(entity.ClrType, "p");
                    BinaryExpression condition = Expression.Equal(Expression.Property(param, isDeleteProp.PropertyInfo), Expression.Constant(false));
                    LambdaExpression filter = Expression.Lambda(condition, param);
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(filter);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }


        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void ClearChangeTracker()
        {
            ChangeTracker.Clear();
        }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }

        public string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }

        public virtual int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using var transaction = Database.BeginTransaction();
                result = Database.ExecuteSqlRaw(sql, parameters);
                transaction.Commit();
            }
            else
                result = Database.ExecuteSqlRaw(sql, parameters);
            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        public bool DatabaseExist()
        {
            return (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
        }

        public void Migrate()
        {
            Database.Migrate();
        }
    }
}

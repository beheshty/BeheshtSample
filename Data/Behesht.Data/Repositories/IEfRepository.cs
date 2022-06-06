using Microsoft.EntityFrameworkCore;
using Behesht.Core.Data;
using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.Repositories
{
    public interface IEfRepository<TEntity> : IRepository<TEntity> where TEntity : IBaseEntity<long>
    {
        DbContext Context { get; }
    }
}

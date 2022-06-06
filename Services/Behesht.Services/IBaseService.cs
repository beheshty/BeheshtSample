using Behesht.Core;
using Behesht.Core.Models;
using Behesht.Core.Models.Paging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Services
{
    public interface IBaseService<TEntity, TModel> where TEntity : BaseEntity<long> where TModel : BaseModel
    {
        PagedList<TModel> Get(PagedListInputMeta meta);
        TModel GetById(long id);
        void Insert(TModel model);
        void Insert(IEnumerable<TModel> models);
        void Update(TModel model);
        void Update(IEnumerable<TModel> models);
        void Delete(long id, bool softDelete = true);
        void Delete(long[] ids, bool softDelete = true);
    }
}

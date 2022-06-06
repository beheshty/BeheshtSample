using Behesht.Core.Data;
using Behesht.Data.Repositories;
using Behesht.Domain.CatalogSample.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.CatalogSample.Repositories.Catalog
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IList<Category> GetHomePageCategories();
    }
}

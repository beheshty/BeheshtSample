using Behesht.Core.Data;
using Behesht.Domain.CatalogSample.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.CatalogSample.Repositories.Catalog
{
    public interface IProductRepository : IRepository<Product>
    {
        IList<Product> GetHomePageProducts();
    }
}

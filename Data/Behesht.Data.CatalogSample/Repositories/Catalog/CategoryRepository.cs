using Behesht.Data.Repositories;
using Behesht.Domain.CatalogSample.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.CatalogSample.Repositories.Catalog
{
    public class CategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public IList<Category> GetHomePageCategories()
        {
            return EntitiesTable.Where(p => p.ShowOnHomePage).ToList();
        }
    }
}

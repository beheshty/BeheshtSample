using Behesht.Data.Repositories;
using Behesht.Domain.CatalogSample.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Data.CatalogSample.Repositories.Catalog
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public override Product FindById(long id)
        {
            return Entities.Include(p=>p.SimilarProducts).Include(p=>p.Categories)
                .Include(p=>p.SpecificationAttributes).FirstOrDefault(p=>p.Id == id);
        }

        public IList<Product> GetHomePageProducts()
        {
            return EntitiesTableNoTracking.Where(p => p.ShowOnHomePage && p.IsActive).ToList();
        }
    }
}

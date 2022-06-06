using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using System.Collections.Generic;

namespace Behesht.Services.CatalogSample.Catalog
{
    public interface IProductService : IBaseService<Product, ProductModel>
    {
        IEnumerable<ProductHomePageModel> GetForHomePage();
    }
}
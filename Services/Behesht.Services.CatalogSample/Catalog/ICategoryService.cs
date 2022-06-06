using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Behesht.Services.CatalogSample.Catalog
{
    public interface ICategoryService : IBaseService<Category, CategoryModel>
    {
        IEnumerable<CategoryModel> GetAll();
        IEnumerable<CategoryHomePageModel> GetHomePageCategoriesTree();
    }
}
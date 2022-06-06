using Mapster;
using MapsterMapper;
using Behesht.Core.Data;
using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using Behesht.Data.CatalogSample.Repositories.Catalog;

namespace Behesht.Services.CatalogSample.Catalog
{
    public class CategoryService : BaseService<Category, CategoryModel>, ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            return _repository.EntitiesTableNoTracking.ProjectToType<CategoryModel>().AsEnumerable();
        }

        public IEnumerable<CategoryHomePageModel> GetHomePageCategoriesTree()
        {
            var categories = _repository.GetHomePageCategories();

            var mainCategories = categories.Where(p => !p.ParentCategoryId.HasValue).ToList();
            var categoriesTree = new List<CategoryHomePageModel>();
            BuildCategoryTree(mainCategories, categoriesTree);

            return categoriesTree;
        }

        private void BuildCategoryTree(List<Category> categories, List<CategoryHomePageModel> categoryTree)
        {
            foreach (var cat in categories)
            {
                var newCat = _mapper.Map<CategoryHomePageModel>(cat);
                categoryTree.Add(newCat);
                if (cat.SubCategories != null && cat.SubCategories.Any())
                {
                    newCat.SubCategories = new List<CategoryHomePageModel>();
                    BuildCategoryTree(cat.SubCategories.ToList(), newCat.SubCategories);
                }
            }
        }
        
    }
}

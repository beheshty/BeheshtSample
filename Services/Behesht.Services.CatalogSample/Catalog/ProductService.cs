using MapsterMapper;
using Behesht.Core.Data;
using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Mapster;
using Behesht.Data.CatalogSample.Repositories.Catalog;

namespace Behesht.Services.CatalogSample.Catalog
{
    public class ProductService : BaseService<Product, ProductModel>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _repository_Category;
        private readonly IRepository<SpecificationAttribute> _repository_SpecificationAttr;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository,
            ICategoryRepository repository_Category,
            IRepository<SpecificationAttribute> repository_SpecificationAttr,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _repository_Category = repository_Category;
            _repository_SpecificationAttr = repository_SpecificationAttr;
            _mapper = mapper;
        }

        public IEnumerable<ProductHomePageModel> GetForHomePage()
        {
            var homePageProducts = _repository.GetHomePageProducts();
            return _mapper.Map<List<ProductHomePageModel>>(homePageProducts);
        }

        public override void Insert(ProductModel model)
        {
            ValidateModel(model);

            Product entity = _mapper.Map<Product>(model);

            //adding similar products
            AddSimilarProducs(model, entity);

            //adding categories
            AddCategories(model, entity);

            _repository.Insert(entity);

            //adding spec attrs
            AddSpecificationAttrs(model);

            model.Id = entity.Id;
        }

        private static void ValidateModel(ProductModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Product Name Should not be null or empty", "ProductName");
            }
            if (model.SpecificationAttrs != null && model.SpecificationAttrs.Any())
            {
                if (model.SpecificationAttrs.Any(s => string.IsNullOrEmpty(s.Name)))
                {
                    throw new ArgumentException("Specification Attribute Name Should not be null or empty", "SpecificationAttributeName");
                }
            }
        }

        private void AddSpecificationAttrs(ProductModel model)
        {
            if (model.SpecificationAttrs != null && model.SpecificationAttrs.Any())
            {
                foreach (var item in model.SpecificationAttrs)
                {
                    var specAttr = _mapper.Map<SpecificationAttribute>(item);
                    _repository_SpecificationAttr.Insert(specAttr);
                    item.Id = specAttr.Id;
                }
            }
        }

        private void AddCategories(ProductModel model, Product entity)
        {
            if (model.CategoryIds != null && model.CategoryIds.Length > 0)
            {
                entity.Categories = new List<Category>();
                var categories = _repository_Category.FindByIds(model.CategoryIds);
                foreach (var item in categories)
                {
                    entity.Categories.Add(item);
                }
            }
        }

        private static void AddSimilarProducs(ProductModel model, Product entity)
        {
            if (model.SimilarProductIds != null && model.SimilarProductIds.Length > 0)
            {
                entity.SimilarProducts = new List<SimilarProduct>();
                foreach (var item in model.SimilarProductIds.Select(p => new SimilarProduct() { SimilarId = p }).ToList())
                {
                    entity.SimilarProducts.Add(item);
                }
            }
        }

        public override void Update(ProductModel model)
        {
            ValidateModel(model);

            var entity = _repository.FindById(model.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{model.Id} Id not found for {nameof(Product)}");
            }

            _mapper.Map(model, entity);

            UpdateSimilarProducts(model, entity);

            UpdateCategories(model, entity);

            _repository.Update(entity);

            UpdateSpecificationAttrs(model, entity);
        }

        private void UpdateSpecificationAttrs(ProductModel model, Product entity)
        {
            foreach (var specAttr in model.SpecificationAttrs)
            {
                if (specAttr.Id == 0)
                {
                    var newSpec = _mapper.Map<SpecificationAttribute>(specAttr);
                    _repository_SpecificationAttr.Insert(newSpec);
                    specAttr.Id = specAttr.Id;
                }
                else
                {
                    var entitySpec = entity.SpecificationAttributes.FirstOrDefault(p => p.Id == specAttr.Id);
                    _mapper.Map(model.SpecificationAttrs, entitySpec);
                    _repository_SpecificationAttr.Update(entitySpec);
                }
            }
            var modelSpecIds = model.SpecificationAttrs.Select(p => p.Id).ToArray();
            var deleteSpecs = entity.SpecificationAttributes.Where(p => !modelSpecIds.Contains(p.Id)).ToList();
            if (deleteSpecs.Any())
            {
                _repository_SpecificationAttr.Delete(deleteSpecs);
            }
        }

        private void UpdateCategories(ProductModel model, Product entity)
        {
            foreach (var cat in entity.Categories.ToList())
            {
                if (model.CategoryIds == null || !model.CategoryIds.Any(p => p == cat.Id))
                {
                    entity.Categories.Remove(cat);
                }
            }
            if (model.CategoryIds != null)
            {
                var entityCatIds = entity.Categories.Select(p => p.Id).ToArray();
                var newCatIds = model.CategoryIds.Where(p => !entityCatIds.Contains(p)).ToArray();
                var categories = _repository_Category.FindByIds(newCatIds);
                foreach (var cat in categories)
                {
                    entity.Categories.Add(cat);
                }
            }
        }

        private static void UpdateSimilarProducts(ProductModel model, Product entity)
        {
            foreach (var similar in entity.SimilarProducts.ToList())
            {
                if (model.SimilarProductIds == null || !model.SimilarProductIds.Any(p => p == similar.SimilarId))
                {
                    entity.SimilarProducts.Remove(similar);
                }
            }
            if (model.SimilarProductIds != null)
            {
                foreach (var similarId in model.SimilarProductIds)
                {
                    if (!entity.SimilarProducts.Any(p => p.SimilarId == similarId))
                    {
                        entity.SimilarProducts.Add(new SimilarProduct()
                        {
                            SimilarId = similarId
                        });
                    }
                }
            }
        }
    }
}

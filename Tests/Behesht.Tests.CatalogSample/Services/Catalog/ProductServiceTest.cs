using Behesht.Core.Data;
using Behesht.Data.CatalogSample.Repositories.Catalog;
using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using MapsterMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Behesht.Tests.CatalogSample.Services.Catalog
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IRepository<SpecificationAttribute>> _specificationAttrRepoMock;

        public ProductServiceTest()
        {
            _repoMock = new Mock<IProductRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _specificationAttrRepoMock = new Mock<IRepository<SpecificationAttribute>>();
        }

        [Fact]
        public void GetById_Success()
        {
            //Arrange
            var fakeProduct = new Product
            {
                Id = 1,
                Description = "Description",
                Name = "Name",
                ShowOnHomePage = true,
                IsActive = true
            };

            _repoMock.Setup(x => x.FindById(1)).Returns(fakeProduct);

            //Act
            var productService = new ProductService(_repoMock.Object, null, null, new Mapper());

            var actualproduct = productService.GetById(1);

            //Assert
            Assert.Equal(fakeProduct.Id, actualproduct.Id);
            Assert.Equal(fakeProduct.Description, actualproduct.Description);
            Assert.Equal(fakeProduct.Name, actualproduct.Name);
            Assert.Equal(fakeProduct.ShowOnHomePage, actualproduct.ShowOnHomePage);
            Assert.Equal(fakeProduct.IsActive, actualproduct.IsActive);
        }

        [Theory]
        [InlineData("product name", "something", "", "", "SpecificationAttributeName")]
        [InlineData("", "something", "Spec Name", "Spec Des", "ProductName")]
        public void Insert_ThrowArgumentException(string productName, string productDescription, string specAttrName, string specAttrDescription, string expectedInvalidParameter)
        {
            var fakeProduct = new ProductModel()
            {
                Name = productName,
                Description = productDescription,
                SpecificationAttrs = new List<SpecificationAttributeForRegisterProductModel>()
                {
                    new SpecificationAttributeForRegisterProductModel()
                    {
                        Name = specAttrName,
                        Description = specAttrDescription
                    }
                }
            };

            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, new Mapper());
            var ex = Record.Exception(() => productService.Insert(fakeProduct));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            if (ex is ArgumentException argEx)
            {
                Assert.Equal(expectedInvalidParameter, argEx.ParamName);
            }
        }

        [Fact]
        public void Insert_Scenario_Success()
        {
            //Arrange
            var fakeCategory = new Category()
            {
                Title = "Title",
                Description = "Description",
                Id = 1
            };
            var fakeSimilarProduct = new SimilarProduct()
            {
                Id = 0,
                MainId = 1,
                SimilarId = 2
            };
            var fakeSpecAttr = new SpecificationAttribute()
            {
                Id = 0,
                Description = "Description",
                Name = "Name",
                ProductId = 0
            };
            var fakeProduct = new Product()
            {
                Id = 0,
                Name = "Name",
                Description = "Description",
                IsActive = true,
                ShowOnHomePage = true,
                Categories = new List<Category>()
                {
                    fakeCategory
                },
                SimilarProducts = new List<SimilarProduct>()
                {
                    fakeSimilarProduct
                },

            };

            _repoMock.Setup(x => x.Insert(fakeProduct))
                .Callback(() => fakeProduct.Id = 1);
            _categoryRepoMock.Setup(x => x.FindByIds(It.Is<long[]>(p => p.Length == 1 && p.Contains(1))))
                .Returns(new List<Category>() { fakeCategory });
            _specificationAttrRepoMock.Setup(x => x.Insert(fakeSpecAttr))
                .Callback(() => fakeSpecAttr.Id = 1);

            var mapperMock = new Mock<Mapper>();
            mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductModel>())).Returns(fakeProduct);
            mapperMock.Setup(x => x.Map<SpecificationAttribute>(It.IsAny<SpecificationAttributeForRegisterProductModel>()))
                .Returns(fakeSpecAttr);

            var productModel = new ProductModel()
            {
                Id = 0,
                Name = "Name",
                Description = "Description",
                IsActive = true,
                ShowOnHomePage = true,
                CategoryIds = new long[] { 1 },
                SimilarProductIds = new long[] { 2 },
                SpecificationAttrs = new List<SpecificationAttributeForRegisterProductModel>()
                {
                    new SpecificationAttributeForRegisterProductModel()
                    {
                        Description = "Description",
                        Id = 0,
                        Name = "Name"
                    }
                }
            };

            //Act
            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, mapperMock.Object);
            productService.Insert(productModel);

            //Assert
            Assert.Equal(1, productModel.Id);
            Assert.Single(productModel.CategoryIds);
            Assert.Equal(1, productModel.CategoryIds[0]);
            Assert.Single(productModel.SpecificationAttrs);
            Assert.Equal(1, productModel.SpecificationAttrs[0].Id);
        }


        [Fact]
        public void Insert_NotZeroId_ShouldThrowInvalidOperationException()
        {
            var product = new ProductModel()
            {
                Id = 1,
                Name = "Name",
                Description = "Description"
            };

            _repoMock.Setup(x => x.Insert(It.Is<Product>(c => c.Id != 0)))
               .Throws<InvalidOperationException>();

            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, new Mapper());

            Assert.Throws<InvalidOperationException>(() => productService.Insert(product));
        }

        [Fact]
        public void Insert_NullModel_ShouldThrowArgNullException()
        {
            ProductModel product = null;

            _repoMock.Setup(x => x.Insert(It.Is<Product>(c => c == null)))
                .Throws<ArgumentNullException>();

            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => productService.Insert(product));
        }

        [Fact]
        public void Update_Scenario_Success()
        {
            //Arrange
            var fakeOldCategory = new Category()
            {
                Title = "Title",
                Description = "Description",
                Id = 1
            };
            var fakeNewCategory = new Category()
            {
                Title = "Title2",
                Description = "Description2",
                Id = 2
            };
            var fakeOldSimilarProduct = new SimilarProduct()
            {
                Id = 1,
                MainId = 1,
                SimilarId = 2
            };
            var fakeOldSpecAttr = new SpecificationAttribute()
            {
                Id = 1,
                Description = "Description",
                Name = "Name",
                ProductId = 1
            };
            var fakeNewSpecAttr = new SpecificationAttribute()
            {
                Id = 0,
                Description = "Description2",
                Name = "Name2",
                ProductId = 1
            };
            var fakeProduct = new Product()
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                IsActive = true,
                ShowOnHomePage = true,
                Categories = new List<Category>()
                {
                    fakeOldCategory
                },
                SimilarProducts = new List<SimilarProduct>()
                {
                    fakeOldSimilarProduct
                },
                SpecificationAttributes = new List<SpecificationAttribute>()
                {
                    fakeOldSpecAttr
                }
            };

            _repoMock.Setup(x => x.FindById(1))
               .Returns(() => fakeProduct);
            _repoMock.Setup(x => x.Update(It.IsAny<Product>()));
            _categoryRepoMock.Setup(x => x.FindByIds(It.Is<long[]>(p => p.Length == 1 && p.Contains(2))))
                .Returns(new List<Category>() { fakeNewCategory });
            _specificationAttrRepoMock.Setup(x => x.Insert(fakeNewSpecAttr))
                .Callback(() =>
                {
                    fakeNewSpecAttr.Id = 2;
                    fakeProduct.SpecificationAttributes.Add(fakeNewSpecAttr);
                });
            _specificationAttrRepoMock.Setup(x => x.Update(fakeOldSpecAttr));

            var mapperMock = new Mock<Mapper>();
            mapperMock.Setup(p => p.Map(It.IsAny<ProductModel>(), fakeProduct)).CallBase();
            mapperMock.Setup(p => p.Map<SpecificationAttribute>(It.IsAny<SpecificationAttributeForRegisterProductModel>()))
                .Returns(fakeNewSpecAttr);

            var productModel = new ProductModel()
            {
                Id = 1,
                Name = "New Name",
                Description = "New Description",
                IsActive = false,
                ShowOnHomePage = false,
                CategoryIds = new long[] { 2 },
                SimilarProductIds = new long[] { 3 },
                SpecificationAttrs = new List<SpecificationAttributeForRegisterProductModel>()
                {
                    new SpecificationAttributeForRegisterProductModel()
                    {
                        Description = "Description",
                        Id = 1,
                        Name = "Name"
                    },
                    new SpecificationAttributeForRegisterProductModel()
                    {
                        Description = "Description2",
                        Id = 0,
                        Name = "Name2"
                    }
                }
            };

            //Act
            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, mapperMock.Object);
            productService.Update(productModel);

            //Assert
            Assert.Equal(productModel.Id, fakeProduct.Id);
            Assert.Equal(productModel.Name, fakeProduct.Name);
            Assert.Equal(productModel.Description, fakeProduct.Description);
            Assert.Equal(productModel.IsActive, fakeProduct.IsActive);
            Assert.Equal(productModel.ShowOnHomePage, fakeProduct.ShowOnHomePage);
            Assert.Single(fakeProduct.Categories);
            Assert.Equal(2, fakeProduct.Categories.First().Id);
            Assert.Single(fakeProduct.SimilarProducts);
            Assert.NotEmpty(fakeProduct.SpecificationAttributes);
            Assert.Equal(2, fakeProduct.SpecificationAttributes.Count);
            Assert.Collection(fakeProduct.SpecificationAttributes, 
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(2, item.Id));
        }

        [Fact]
        public void Update_NullModel_ShouldThrowArgNullException()
        {
            ProductModel product = null;

            _repoMock.Setup(x => x.Update(It.Is<Product>(c => c == null)))
                .Throws<ArgumentNullException>();

            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => productService.Update(product));
        }

        [Fact]
        public void Update_KeyNotFoundException()
        {
            var fakeProduct = new ProductModel()
            {
                Name = "Name",
                Id = 1
            };

            _repoMock.Setup(x => x.FindById(1)).Returns(() => null);

            var productService = new ProductService(_repoMock.Object, _categoryRepoMock.Object, _specificationAttrRepoMock.Object, new Mapper());
            Assert.Throws<KeyNotFoundException>(() => productService.Update(fakeProduct));
        }

    }
}

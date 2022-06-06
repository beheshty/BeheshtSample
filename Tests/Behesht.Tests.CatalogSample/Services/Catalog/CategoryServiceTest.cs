using Behesht.Data.CatalogSample.Repositories.Catalog;
using Behesht.Data.Repositories;
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
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _repoMock;

        public CategoryServiceTest()
        {
            _repoMock = new Mock<ICategoryRepository>();
        }

        [Fact]
        public void GetHomePageCategoriesTree_Success()
        {
            //Arrange
            var flatCategories = GetFlatCategories();

            _repoMock.Setup(x => x.GetHomePageCategories()).Returns(flatCategories);

            //Act
            var categoryService = new CategoryService(_repoMock.Object, new Mapper());
            var categoriesTree = categoryService.GetHomePageCategoriesTree();

            //Assert
            Assert.Single(categoriesTree);
            var firstCategory = categoriesTree.First();
            Assert.Equal(1, firstCategory.Id);
            Assert.Single(firstCategory.SubCategories);
            var secondCategory = firstCategory.SubCategories.First();
            Assert.Equal(2, secondCategory.Id);
            Assert.Single(secondCategory.SubCategories);
            var thirdCategory = secondCategory.SubCategories.First();
            Assert.Equal(3, thirdCategory.Id);
        }


        [Fact]
        public void GetById_Success()
        {
            //Arrange
            var fakeCategory = new Category
            {
                Id = 1,
                Description = "Description",
                Title = "Title",
            };

            _repoMock.Setup(x => x.FindById(1)).Returns(fakeCategory);

            //Act
            var categoryService = new CategoryService(_repoMock.Object, new Mapper());

            var actualcategory = categoryService.GetById(1);

            //Assert
            Assert.Equal(fakeCategory.Id, actualcategory.Id);
            Assert.Equal(fakeCategory.Description, actualcategory.Description);
            Assert.Equal(fakeCategory.Title, actualcategory.Title);
        }

        [Fact]
        public void Insert_ModelAndEntityIdShouldBeTheSameAfterInsert()
        {
            //Arrange
            var fakeCategory = new Category()
            {
                Id = 0,
                Title = "Title",
            };

            _repoMock.Setup(x => x.Insert(fakeCategory))
                .Callback(() => fakeCategory.Id = 1);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CategoryModel, Category>(It.IsAny<CategoryModel>())).Returns(fakeCategory);

            var categoryModel = new CategoryModel()
            {
                Id = 0,
                Title = "Title",
            };

            //Act
            var categoryService = new CategoryService(_repoMock.Object, mapperMock.Object);
            categoryService.Insert(categoryModel);

            //Assert
            Assert.Equal(1, categoryModel.Id);
        }


        [Fact]
        public void Insert_NotZeroId_ShouldThrowInvalidOperationException()
        {
            _repoMock.Setup(x => x.Insert(It.Is<Category>(c => c.Id != 0)))
                .Throws<InvalidOperationException>();

            var category = new CategoryModel()
            {
                Id = 1,
                Title = "Title",
            };

            var categoryService = new CategoryService(_repoMock.Object, new Mapper());

            Assert.Throws<InvalidOperationException>(() => categoryService.Insert(category));
        }

        [Fact]
        public void Insert_NullModel_ShouldThrowArgNullException()
        {
            _repoMock.Setup(x => x.Insert(It.Is<Category>(c => c == null)))
                .Throws<ArgumentNullException>();

            CategoryModel category = null;

            var categoryService = new CategoryService(_repoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => categoryService.Insert(category));
        }

        [Fact]
        public void Update_NullModel_ShouldThrowArgNullException()
        {
            _repoMock.Setup(x => x.Update(It.Is<Category>(c => c == null)))
                .Throws<ArgumentNullException>();

            CategoryModel category = null;

            var categoryService = new CategoryService(_repoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => categoryService.Update(category));
        }


        private static List<Category> GetFlatCategories()
        {
            var cat1 = new Category()
            {
                Id = 1,
                Description = "Description1",
                ParentCategoryId = null,
                ParentCategory = null,
                Title = "Title1",
                ShowOnHomePage = true
            };
            var cat2 = new Category()
            {
                Id = 2,
                ParentCategory = cat1,
                ParentCategoryId = 1,
                ShowOnHomePage = true,
                Title = "Title2",
                Description = "Description2",
            };
            var cat3 = new Category()
            {
                Id = 3,
                ParentCategory = cat2,
                ParentCategoryId = 2,
                ShowOnHomePage = true,
                Title = "Title3",
                Description = "Description3",
            };
            cat1.SubCategories.Add(cat2);
            cat2.SubCategories.Add(cat3);

            return new List<Category>() { cat1, cat2, cat3 };
        }
    }
}

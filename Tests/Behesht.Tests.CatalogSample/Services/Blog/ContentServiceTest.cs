using Behesht.Core.Data;
using Behesht.Domain.CatalogSample.Blog;
using Behesht.Services.CatalogSample.Blog;
using Behesht.Services.CatalogSample.Models.Blog;
using MapsterMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Behesht.Tests.CatalogSample.Services.Blog
{
    public class ContentServiceTest
    {
        private readonly Mock<IRepository<Content>> _repoMock;
        public ContentServiceTest()
        {
            _repoMock = new Mock<IRepository<Content>>();
        }

        [Fact]
        public void GetById_Success()
        {
            //Arrange
            var fakeContent = new Content
            {
                Id = 1,
                Description = "Description",
                Title = "Title",
            };

            _repoMock.Setup(x => x.FindById(1)).Returns(fakeContent);

            //Act
            var contentService = new ContentService(_repoMock.Object, new Mapper());

            var actualContent = contentService.GetById(1);

            //Assert
            Assert.Equal(fakeContent.Id, actualContent.Id);
            Assert.Equal(fakeContent.Description, actualContent.Description);
            Assert.Equal(fakeContent.Title, actualContent.Title);
        }

        [Fact]
        public void Insert_ModelAndEntityIdShouldBeTheSameAfterInsert()
        {
            //Arrange
            var fakeContent = new Content()
            {
                Id = 0,
                Title = "Title",
            };

            _repoMock.Setup(x => x.Insert(fakeContent))
                .Callback(() => fakeContent.Id = 1);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<ContentModel, Content>(It.IsAny<ContentModel>())).Returns(fakeContent);

            var contentModel = new ContentModel()
            {
                Id = 0,
                Title = "Title",
            };

            //Act
            var contentService = new ContentService(_repoMock.Object, mapperMock.Object);
            contentService.Insert(contentModel);

            //Assert
            Assert.Equal(1, contentModel.Id);
        }


        [Fact]
        public void Insert_NotZeroId_ShouldThrowInvalidOperationException()
        {
            _repoMock.Setup(x => x.Insert(It.Is<Content>(c => c.Id != 0)))
                .Throws<InvalidOperationException>();

            var content = new ContentModel()
            {
                Id = 1,
                Title = "Title",
            };

            var contentService = new ContentService(_repoMock.Object, new Mapper());

            Assert.Throws<InvalidOperationException>(() => contentService.Insert(content));
        }

        [Fact]
        public void Insert_NullModel_ShouldThrowArgNullException()
        {
            _repoMock.Setup(x => x.Insert(It.Is<Content>(c => c == null)))
                .Throws<ArgumentNullException>();

            ContentModel content = null;

            var contentService = new ContentService(_repoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => contentService.Insert(content));
        }

        [Fact]
        public void Update_NullModel_ShouldThrowArgNullException()
        {
            _repoMock.Setup(x => x.Update(It.Is<Content>(c => c == null)))
                .Throws<ArgumentNullException>();

            ContentModel content = null;

            var contentService = new ContentService(_repoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => contentService.Update(content));
        }
    }
}

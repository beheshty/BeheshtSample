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
    public class SlideShowServiceTest
    {
        private readonly Mock<IRepository<SlideShow>> _repoMock;
        public SlideShowServiceTest()
        {
            _repoMock = new Mock<IRepository<SlideShow>>();
        }

        [Fact]
        public void GetById_Success()
        {
            //Arrange
            var fakeSlidshow = new SlideShow
            {
                Id = 1,
                Description = "Description",
                Title = "Title",
            };

            _repoMock.Setup(x => x.FindById(1)).Returns(fakeSlidshow);

            //Act
            var slidshowService = new SlideShowService(_repoMock.Object, new Mapper());

            var actualslidshow = slidshowService.GetById(1);

            //Assert
            Assert.Equal(fakeSlidshow.Id, actualslidshow.Id);
            Assert.Equal(fakeSlidshow.Description, actualslidshow.Description);
            Assert.Equal(fakeSlidshow.Title, actualslidshow.Title);
        }

        [Fact]
        public void Insert_ModelAndEntityIdShouldBeTheSameAfterInsert()
        {
            //Arrange
            var fakeslidshow = new SlideShow()
            {
                Id = 0,
                Title = "Title",
            };

            _repoMock.Setup(x => x.Insert(fakeslidshow))
                .Callback(() => fakeslidshow.Id = 1);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<SlideShowModel, SlideShow>(It.IsAny<SlideShowModel>())).Returns(fakeslidshow);

            var slidshowModel = new SlideShowModel()
            {
                Id = 0,
                Title = "Title",
            };

            //Act
            var slidshowService = new SlideShowService(_repoMock.Object, mapperMock.Object);
            slidshowService.Insert(slidshowModel);

            //Assert
            Assert.Equal(1, slidshowModel.Id);
        }


        [Fact]
        public void Insert_NotZeroId_ShouldThrowInvalidOperationException()
        {
            _repoMock.Setup(x => x.Insert(It.Is<SlideShow>(c => c.Id != 0)))
                .Throws<InvalidOperationException>();

            var slidshow = new SlideShowModel()
            {
                Id = 1,
                Title = "Title",
            };

            var slidshowService = new SlideShowService(_repoMock.Object, new Mapper());

            Assert.Throws<InvalidOperationException>(() => slidshowService.Insert(slidshow));
        }

        [Fact]
        public void Insert_NullModel_ShouldThrowArgNullException()
        {
            _repoMock.Setup(x => x.Insert(It.Is<SlideShow>(c => c == null)))
                .Throws<ArgumentNullException>();

            SlideShowModel slidshow = null;

            var slidshowService = new SlideShowService(_repoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => slidshowService.Insert(slidshow));
        }

        [Fact]
        public void Update_NullModel_ShouldThrowArgNullException()
        {
            _repoMock.Setup(x => x.Update(It.Is<SlideShow>(c => c == null)))
                .Throws<ArgumentNullException>();

            SlideShowModel slidshow = null;

            var slidshowService = new SlideShowService(_repoMock.Object, new Mapper());

            Assert.Throws<ArgumentNullException>(() => slidshowService.Update(slidshow));
        }
    }
}

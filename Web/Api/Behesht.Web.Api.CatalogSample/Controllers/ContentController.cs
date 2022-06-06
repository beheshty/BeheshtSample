using Microsoft.AspNetCore.Http;
using Behesht.Domain.CatalogSample.Blog;
using Behesht.Services;
using Behesht.Services.CatalogSample.Blog;
using Behesht.Services.CatalogSample.Models.Blog;
using Behesht.Web.Framework.Controllers;

namespace Behesht.Web.Api.CatalogSample.Controllers
{
    public class ContentController : BaseApiController<Content, ContentModel>
    {
        public ContentController(IContentService entityService, IHttpContextAccessor httpContextAccessor) : base(entityService, httpContextAccessor)
        {
        }
    }
}

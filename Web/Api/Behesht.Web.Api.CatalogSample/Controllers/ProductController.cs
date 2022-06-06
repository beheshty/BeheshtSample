using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services;
using Behesht.Services.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using Behesht.Web.Framework.Controllers;

namespace Behesht.Web.Api.CatalogSample.Controllers
{
    public class ProductController : BaseApiController<Product, ProductModel>
    {
        private readonly IProductService _entityService;

        public ProductController(IProductService entityService, IHttpContextAccessor httpContextAccessor) : base(entityService, httpContextAccessor)
        {
            _entityService = entityService;
        }

        [HttpGet("[action]")]
        public IActionResult GetForHomePage()
        {
            return Ok(_entityService.GetForHomePage());
        }
    }
}

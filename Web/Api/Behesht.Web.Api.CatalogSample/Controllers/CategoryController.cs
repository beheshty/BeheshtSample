using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Behesht.Domain.CatalogSample.Catalog;
using Behesht.Services;
using Behesht.Services.CatalogSample.Catalog;
using Behesht.Services.CatalogSample.Models.Catalog;
using Behesht.Web.Framework.Controllers;
using System.Threading.Tasks;

namespace Behesht.Web.Api.CatalogSample.Controllers
{
    public class CategoryController : BaseApiController<Category, CategoryModel>
    {
        private readonly ICategoryService _entityService;

        public CategoryController(ICategoryService entityService, IHttpContextAccessor httpContextAccessor) : base(entityService, httpContextAccessor)
        {
            _entityService = entityService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            return Ok(_entityService.GetAll());
        }


        [HttpGet("[action]")]
        public  IActionResult GetTree()
        {
            return Ok(_entityService.GetHomePageCategoriesTree());
        }

    }
}

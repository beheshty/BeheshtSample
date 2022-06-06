using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Behesht.Core;
using Behesht.Core.Models;
using Behesht.Core.Models.Paging;
using Behesht.Services;
using Behesht.Services.Models;
using Behesht.Web.Framework.Http.Extensions;
using Behesht.Web.Framework.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Behesht.Web.Framework.Controllers
{
    public class BaseApiController<TEntity, TModel> : BaseApiController where TEntity : BaseEntity<long> where TModel : BaseModel
    {
        private readonly IBaseService<TEntity, TModel> _entityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseApiController(IBaseService<TEntity, TModel> entityService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _entityService = entityService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public virtual IActionResult Get()
        {
            var pagedList = _entityService.Get(GetPagedListInput());

            return Ok(pagedList);
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(long id)
        {
            var entity = _entityService.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public virtual IActionResult Post(TModel model)
        {
            if (model.Id != 0)
            {
                return BadRequest(new BaseApiResult()
                {
                    Status = 401,
                    Message = ServicesCommonHelper.DefaultLocalizer.TranslateByKey("idMustBeZero"),
                    Success = false
                });
            }
            _entityService.Insert(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public virtual IActionResult Put(long id, TModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(new BaseApiResult()
                {
                    Status = 401,
                    Message = ServicesCommonHelper.DefaultLocalizer.TranslateByKey("sameModelAndRouteId"),
                    Success = false
                });
            }
            _entityService.Update(model);
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(long id)
        {
            _entityService.Delete(id);
            return Ok(null);
        }

        [NonAction]
        public BeheshtObjectResult Behesht(BaseServiceResult<TModel> result)
        {
            var baseApiResult = new BaseApiResult()
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success,
                MessageLocalizationKey = result.MessageLocalizationKey,
                Status = result.Status
            };

            return Behesht(baseApiResult);
        }


    }


    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseApiController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #region ok region

        [NonAction]
        public OkObjectResult Ok<T>(PagedList<T> pagedList)
        {
            return base.Ok(pagedList);
        }

        [NonAction]
        public OkObjectResult Ok(object value, string message)
        {
            return base.Ok(new BaseApiResult
            {
                Data = value,
                Message = message,
                Status = (int)ResponseStatusType.Success,
                Success = true
            });
        }

        [NonAction]
        public OkObjectResult Ok(BaseApiResult result)
        {
            return base.Ok(result);
        }

        [NonAction]
        public new OkObjectResult Ok()
        {
            return base.Ok(new BaseApiResult
            {
                Data = null,
                Message = "",
                Status = (int)ResponseStatusType.Success,
                Success = true
            });
        }

        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return base.Ok(new BaseApiResult
            {
                Data = value,
                Message = "",
                Status = (int)ResponseStatusType.Success,
                Success = true
            });
        }

        #endregion

        #region badrequest region


        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
        {
            return base.BadRequest(new BaseApiResult
            {
                Data = error,
                Message = "",
                Status = (int)ResponseStatusType.BadRequest,
                Success = false
            });
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(string message)
        {
            return base.BadRequest(new BaseApiResult
            {
                Data = null,
                Message = message,
                Status = (int)ResponseStatusType.BadRequest,
                Success = false
            });
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(BaseApiResult result)
        {
            return base.BadRequest(result);
        }

        [NonAction]
        public new BadRequestObjectResult BadRequest()
        {
            return base.BadRequest(new BaseApiResult
            {
                Data = null,
                Message = "",
                Status = (int)ResponseStatusType.BadRequest,
                Success = false
            });
        }

        #endregion


        [NonAction]
        public PagedListInputMeta GetPagedListInput()
        {
            var meta = _httpContextAccessor.HttpContext.Request.GetFromQueryString<PagedListInputMeta>();
            AddColumnFiltersToMeta(meta);
            return meta;
        }

        private void AddColumnFiltersToMeta(PagedListInputMeta meta)
        {
            var queries = HttpContext.Request.QueryString.Value;
            var regex = new Regex(@"(columnFilters)\[(\d)\].(columnName|search|searchType)\=([^&]*)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(queries);
            if (matches.Count > 0)
            {
                foreach (var regMatch in matches.GroupBy(p => p.Groups[2].Value))
                {
                    var columnName = regMatch.FirstOrDefault(p => p.Groups[3].ToString().ToLower() == "columnname")?.Groups[4].Value;
                    var search = regMatch.FirstOrDefault(p => p.Groups[3].ToString().ToLower() == "search")?.Groups[4].Value;
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(columnName))
                    {
                        var searchTypeStr = regMatch.FirstOrDefault(p => p.Groups[3].ToString().ToLower() == "searchtype")?.Groups[4].Value;
                        if (!Enum.TryParse(searchTypeStr, out SearchType searchType))
                        {
                            searchType = SearchType.Like;
                        }
                        meta.ColumnFilters.Add(new ColumnFilter()
                        {
                            ColumnName = columnName,
                            Search = search,
                            SearchType = searchType
                        });
                    }
                }
            }

        }

        [NonAction]
        public BeheshtObjectResult Behesht(BaseApiResult result)
        {
            if (!string.IsNullOrEmpty(result.MessageLocalizationKey))
            {
                result.Message = ServicesCommonHelper.DefaultLocalizer.TranslateByKey(result.MessageLocalizationKey);
            }
            if (result.Success)
            {
                return new BeheshtObjectResult(result)
                {
                    StatusCode = 200
                };
            }
            return new BeheshtObjectResult(result)
            {
                StatusCode = 400
            };
        }

        [NonAction]
        public BeheshtObjectResult Behesht<TData>(BaseServiceResult<TData> result) where TData : class
        {
            var baseApiResult = new BaseApiResult()
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success,
                MessageLocalizationKey = result.MessageLocalizationKey,
                Status = result.Status
            };

            return Behesht(baseApiResult);
        }

        [NonAction]
        public BeheshtObjectResult Behesht(BaseServiceResult result)
        {
            var baseApiResult = new BaseApiResult()
            {
                Message = result.Message,
                Success = result.Success,
                MessageLocalizationKey = result.MessageLocalizationKey,
                Status = result.Status
            };

            return Behesht(baseApiResult);
        }

    }
}

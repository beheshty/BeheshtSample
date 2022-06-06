using MapsterMapper;
using Behesht.Core.Data;
using Behesht.Domain.CatalogSample.Blog;
using Behesht.Services.CatalogSample.Models.Blog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Services.CatalogSample.Blog
{
    public class ContentService : BaseService<Content, ContentModel>, IContentService
    {
        public ContentService(IRepository<Content> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}

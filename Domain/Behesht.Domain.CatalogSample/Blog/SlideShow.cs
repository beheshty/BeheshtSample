using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Domain.CatalogSample.Blog
{
    public class SlideShow : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Media { get; set; } = string.Empty;
    }
}

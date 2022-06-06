using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Domain.CatalogSample.Blog
{
    public class Content : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PageKey { get; set; } = string.Empty;
        public string PositionKey { get; set; } = string.Empty;
        public string[] Media { get; set; } = Array.Empty<string>();
    }
}

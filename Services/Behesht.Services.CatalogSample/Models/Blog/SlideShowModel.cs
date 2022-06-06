using Behesht.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Services.CatalogSample.Models.Blog
{
    public class SlideShowModel : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Media { get; set; } = string.Empty;
    }
}

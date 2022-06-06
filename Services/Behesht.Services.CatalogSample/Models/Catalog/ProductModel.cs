using Behesht.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Services.CatalogSample.Models.Catalog
{
    public class ProductModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string MainMedia { get; set; }
        public string[] Media { get; set; } = Array.Empty<string>();
        public long[] SimilarProductIds { get; set; } = Array.Empty<long>();
        public long[] CategoryIds { get; set; } = Array.Empty<long>();
        public List<SpecificationAttributeForRegisterProductModel> SpecificationAttrs { get; set; }
    }
}

using Behesht.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Services.CatalogSample.Models.Catalog
{
    public class SpecificationAttributeModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long ProductId { get; set; }

    }
}

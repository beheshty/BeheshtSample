using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Domain.CatalogSample.Catalog
{
    public class SpecificationAttribute : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long ProductId { get; set; }

        //
        //navigations
        //

        public Product Product { get; set; }
    }
}

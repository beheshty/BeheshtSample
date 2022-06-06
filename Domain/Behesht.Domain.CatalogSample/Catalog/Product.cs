using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Domain.CatalogSample.Catalog
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string MainMedia { get; set; }
        public string[] Media { get; set; } = Array.Empty<string>();

        //
        //navigations
        //

        public ICollection<Category> Categories  { get; set; } = new HashSet<Category>();
        public ICollection<SpecificationAttribute> SpecificationAttributes { get; set; } = new HashSet<SpecificationAttribute>();
        public ICollection<SimilarProduct> SimilarProducts { get; set; } = new HashSet<SimilarProduct>();
        public ICollection<SimilarProduct> MainProducts { get; set; } = new HashSet<SimilarProduct>();

    }
}

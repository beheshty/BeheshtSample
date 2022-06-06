using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Domain.CatalogSample.Catalog
{
    public class Category : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool ShowOnHomePage { get; set; }
        public string[] Media { get; set; } = Array.Empty<string>();
        public long? ParentCategoryId { get; set; }

        //
        //navigations
        //

        public ICollection<Category> SubCategories { get; set; } = new HashSet<Category>();
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public Category? ParentCategory { get; set; }
    }
}

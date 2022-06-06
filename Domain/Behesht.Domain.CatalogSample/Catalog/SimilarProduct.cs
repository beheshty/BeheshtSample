using Behesht.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Domain.CatalogSample.Catalog
{
    public class SimilarProduct : BaseEntity
    {
        public long MainId { get; set; }
        public long SimilarId { get; set; }

        //
        //navigations
        //

        public Product Main { get; set; }
        public Product Similar { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Behesht.Domain.CatalogSample.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Data.CatalogSample.Configurations.Catalog
{
    public class SpecificationAttributeConfig : IEntityTypeConfiguration<SpecificationAttribute>
    {
        public void Configure(EntityTypeBuilder<SpecificationAttribute> builder)
        {
            
        }
    }
}

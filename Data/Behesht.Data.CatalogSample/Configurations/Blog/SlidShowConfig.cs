using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Behesht.Domain.CatalogSample.Blog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Data.CatalogSample.Configurations.Blog
{
    public class SlidShowConfig : IEntityTypeConfiguration<SlideShow>
    {
        public void Configure(EntityTypeBuilder<SlideShow> builder)
        {
            
        }
    }
}

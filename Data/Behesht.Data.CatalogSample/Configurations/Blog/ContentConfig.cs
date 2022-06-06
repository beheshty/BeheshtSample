using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Behesht.Domain.CatalogSample.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behesht.Data.CatalogSample.Configurations.Blog
{
    public class ContentConfig : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.Property(p => p.Media).HasConversion(p => string.Join(',', p), p => p.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray(), 
                new ValueComparer<string[]>((p1, p2) => p1.Intersect(p2).Count() == p2.Length, p => p.GetHashCode(), p => p));
        }
    }
}

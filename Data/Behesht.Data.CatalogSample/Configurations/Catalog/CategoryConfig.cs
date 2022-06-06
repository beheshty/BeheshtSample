using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Behesht.Domain.CatalogSample.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behesht.Data.CatalogSample.Configurations.Catalog
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(c => c.SubCategories).WithOne(sc => sc.ParentCategory).HasForeignKey(c => c.ParentCategoryId);
            builder.HasMany(c => c.Products).WithMany(p => p.Categories);

            builder.Property(p => p.Media).HasConversion(p => string.Join(',', p), p => p.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray(),
                new ValueComparer<string[]>((p1, p2) => p1.Intersect(p2).Count() == p2.Length, p => p.GetHashCode(), p => p));
        }
    }
}

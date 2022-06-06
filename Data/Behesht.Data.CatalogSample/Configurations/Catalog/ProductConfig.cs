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
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(p => p.SpecificationAttributes).WithOne(p => p.Product).HasForeignKey(p => p.ProductId);

            builder.HasMany(p => p.SimilarProducts).WithOne(p => p.Similar).HasForeignKey(p => p.SimilarId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(p => p.MainProducts).WithOne(p => p.Main).HasForeignKey(p => p.MainId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Media).HasConversion(p => string.Join(',', p), p => p.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray(),
                new ValueComparer<string[]>((p1, p2) => p1.Intersect(p2).Count() == p2.Length, p => p.GetHashCode(), p => p));
        }
    }
}

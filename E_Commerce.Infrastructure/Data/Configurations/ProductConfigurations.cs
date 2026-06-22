using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(e => e.ProductBrand)
                   .WithMany()
                   .HasForeignKey(e => e.BrandId);

            builder.HasOne(e => e.ProductType)
                   .WithMany()
                   .HasForeignKey(e => e.TypeId);

            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.PictureUrl).HasMaxLength(200);
            builder.Property(e => e.Price).HasColumnType("decimal(18,2)");
        }
    }
}

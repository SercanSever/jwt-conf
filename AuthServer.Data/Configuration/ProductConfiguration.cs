using AuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Configuration
{
   public class ProductConfiguration : IEntityTypeConfiguration<Product>
   {
      public void Configure(EntityTypeBuilder<Product> builder)
      {
         builder.HasKey(p => p.Id);
         builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
         builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
         builder.Property(p => p.Stock).HasDefaultValue(0).IsRequired();
         builder.Property(p => p.UserId).IsRequired();
      }
   }
}
using AuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Configuration
{
   public class RefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
   {
      public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
      {
         builder.HasKey(p => p.UserId);
         builder.Property(p => p.Code).HasMaxLength(500).IsRequired();
         builder.Property(p => p.Expiration).IsRequired();
      }
   }
}
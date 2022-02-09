using AuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Configuration
{
   public class UserConfiguration : IEntityTypeConfiguration<User>
   {
      public void Configure(EntityTypeBuilder<User> builder)
      {
         builder.Property(u => u.City).HasMaxLength(50);
      }
   }
}
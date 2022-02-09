using AuthServer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data
{
   public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
   {
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {
      }
      public DbSet<UserRefreshToken> RefreshTokens { get; set; }
      public DbSet<Product> Products { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
         base.OnModelCreating(builder);
      }

   }
}
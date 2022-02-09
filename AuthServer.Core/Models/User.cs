using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models
{
   public class User : IdentityUser
   {
      public string? City { get; set; }
   }
}
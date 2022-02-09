using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Services
{
   public static class SignService
   {
      public static SecurityKey GetSigningKey(string key)
      {
         return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      }
   }
}
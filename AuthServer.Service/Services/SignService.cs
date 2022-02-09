using System.Text;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Service.Services
{
   public static class SignService
   {
      public static SecurityKey GetSigningKey(string key)
      {
         return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      }
   }
}
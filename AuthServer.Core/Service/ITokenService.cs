using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;

namespace AuthServer.Core.Service
{
   public interface ITokenService
   {
      TokenDto CreateToken(User user);
      ClientTokenDto CreateClientToken(Client client);
   }
}
using AuthServer.Core.Dto;
using Shared.Dto;

namespace AuthServer.Core.Service
{
   public interface IAuthenticationService
   {
      Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
      Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
      Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken);
      Response<ClientTokenDto> CreateClientTokenAsync(ClientLoginDto clientLoginDto);
   }
}
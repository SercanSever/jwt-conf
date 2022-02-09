using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Repository;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Dto;

namespace AuthServer.Service.Services
{
   public class AuthenticationService : IAuthenticationService
   {
      private readonly List<Client> _clients;
      private readonly ITokenService _tokenService;
      private readonly UserManager<User> _userManager;
      private readonly IUnitOfWork _unitOfWork;
      private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;
      public AuthenticationService(ITokenService tokenService, UserManager<User> userManager, IOptions<List<Client>> clients, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
      {
         _tokenService = tokenService;
         _userManager = userManager;
         _clients = clients.Value;
         _unitOfWork = unitOfWork;
         _userRefreshTokenService = userRefreshTokenService;
      }
      public Response<ClientTokenDto> CreateClientToken(ClientLoginDto clientLoginDto)
      {
         var client = _clients.FirstOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
         if (client == null) return Response<ClientTokenDto>.Fail("Client not found", true, 404);

         var token = _tokenService.CreateClientToken(client);

         return Response<ClientTokenDto>.Success(token, 200);
      }

      public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
      {
         if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

         var user = await _userManager.FindByEmailAsync(loginDto.Email);

         if (user == null) return Response<TokenDto>.Fail("Email or password is incorrect", true, 404);
         if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Response<TokenDto>.Fail("Email or password is incorrect", true, 404);

         var token = _tokenService.CreateToken(user);
         var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

         if (userRefreshToken == null)
         {
            await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
         }
         else
         {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
         }
         await _unitOfWork.CommitAsync();

         return Response<TokenDto>.Success(token, 200);
      }

      public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
      {
         if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

         var userRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

         if (userRefreshToken == null) return Response<TokenDto>.Fail("Refresh token not found", true, 404);

         var user = await _userManager.FindByIdAsync(userRefreshToken.UserId);

         if (user == null) return Response<TokenDto>.Fail("User not found", true, 404);

         var token = _tokenService.CreateToken(user);

         userRefreshToken.Code = token.RefreshToken;
         userRefreshToken.Expiration = token.RefreshTokenExpiration;

         await _unitOfWork.CommitAsync();

         return Response<TokenDto>.Success(token, 200);
      }

      public async Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
      {
         var userRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
         if (userRefreshToken == null) return Response<NoDataDto>.Fail("Refresh token not found", true, 404);

         _userRefreshTokenService.Remove(userRefreshToken);

         await _unitOfWork.CommitAsync();

         return Response<NoDataDto>.Success(200);
      }
   }
}
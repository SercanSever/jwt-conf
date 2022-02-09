using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using Shared.Services;

namespace AuthServer.Service.Services
{
   public class TokenService : ITokenService
   {
      private readonly UserManager<User> _userManager;
      private readonly CustomTokenOptions _tokenOptions;

      public TokenService(UserManager<User> userManager, IOptions<CustomTokenOptions> tokenOptions)
      {
         _userManager = userManager;
         _tokenOptions = tokenOptions.Value;
      }

      public ClientTokenDto CreateClientToken(Client client)
      {
         var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
         var securityKey = SignService.GetSigningKey(_tokenOptions.SecurityKey);
         var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
         JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            claims: this.GetClaimsByClient(client),
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials);

         var handler = new JwtSecurityTokenHandler();
         var token = handler.WriteToken(jwtSecurityToken);
         var clientTokenDto = new ClientTokenDto
         {
            AccessToken = token,
            AccessTokenExpiration = accessTokenExpiration,
         };
         return clientTokenDto;
      }

      public TokenDto CreateToken(User user)
      {
         var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
         var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);
         var securityKey = SignService.GetSigningKey(_tokenOptions.SecurityKey);
         var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
         JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            claims: this.GetClaims(user, _tokenOptions.Audience),
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials);

         var handler = new JwtSecurityTokenHandler();
         var token = handler.WriteToken(jwtSecurityToken);
         var tokenDto = new TokenDto
         {
            AccessToken = token,
            RefreshToken = this.CreateRefreshToken(),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
         };
         return tokenDto;
      }
      //Token's Payload
      private IEnumerable<Claim> GetClaims(User user, List<string> audience)
      {
         var userList = new List<Claim>();
         userList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
         userList.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
         userList.Add(new Claim(ClaimTypes.Name, user.UserName));
         userList.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
         userList.AddRange(audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
         return userList;
      }
      private IEnumerable<Claim> GetClaimsByClient(Client client)
      {
         var clientList = new List<Claim>();
         clientList.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
         clientList.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
         clientList.Add(new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()));
         return clientList;
      }

      private string CreateRefreshToken()
      {
         var numberByte = new byte[32];
         using var random = RandomNumberGenerator.Create();
         random.GetBytes(numberByte);
         return Convert.ToBase64String(numberByte);
      }
   }
}
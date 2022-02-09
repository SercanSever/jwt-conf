using AuthServer.Core.Dto;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class AuthController : CustomBaseController
   {
      private readonly IAuthenticationService _authenticationService;

      public AuthController(IAuthenticationService authenticationService)
      {
         _authenticationService = authenticationService;
      }

      [HttpPost("createtoken")]
      public async Task<IActionResult> CreateToken(LoginDto loginDto)
      {
         var result = await _authenticationService.CreateTokenAsync(loginDto);
         return ActionResultInstance(result);
      }

      [HttpPost("createclienttoken")]
      public IActionResult CreateClientToken(ClientLoginDto clientLoginDto)
      {
         var result = _authenticationService.CreateClientToken(clientLoginDto);
         return ActionResultInstance(result);
      }
      [HttpPost("revokerefreshtoken")]
      public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
      {
         var result = await _authenticationService.RevokeRefreshTokenAsync(refreshTokenDto.RefreshToken);
         return ActionResultInstance(result);
      }
      [HttpPost("createtokenbyrefreshtoken")]
      public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
      {
         var result = await _authenticationService.CreateTokenByRefreshTokenAsync(refreshTokenDto.RefreshToken);
         return ActionResultInstance(result);
      }
   }
}
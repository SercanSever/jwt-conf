using AuthServer.Core.Dto;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class UserController : CustomBaseController
   {
      private readonly IUserService _userService;

      public UserController(IUserService userService)
      {
         _userService = userService;
      }

      [HttpPost("createuser")]
      public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
      {
         var result = await _userService.CreateUserAsync(createUserDto);
         return ActionResultInstance(result);
      }
      [Authorize]
      [HttpGet("getuser")]
      public async Task<IActionResult> GetUser()
      {
         var result = await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name);
         return ActionResultInstance(result);
      }
   }
}
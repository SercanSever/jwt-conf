using AuthServer.Core.Dto;
using Shared.Dto;

namespace AuthServer.Core.Service
{
   public interface IUserService
   {
      Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
      Task<Response<UserDto>> GetUserByNameAsync(string userName);
   }
}
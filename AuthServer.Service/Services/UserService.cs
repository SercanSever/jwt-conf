using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mapper;
using Microsoft.AspNetCore.Identity;
using Shared.Dto;

namespace AuthServer.Service.Services
{
   public class UserService : IUserService
   {
      private readonly UserManager<User> _userManager;
      private readonly IUnitOfWork _unitOfWork;
      public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork)
      {
         _userManager = userManager;
         _unitOfWork = unitOfWork;
      }
      public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
      {
         var user = new User();
         user.UserName = createUserDto.UserName;
         user.Email = createUserDto.Email;

         var result = await _userManager.CreateAsync(user, createUserDto.Password);

         if (!result.Succeeded) return Response<UserDto>.Fail(new ErrorDto(result.Errors.Select(x => x.Description).ToList(), true), 400);

         await _unitOfWork.CommitAsync();

         return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
      }

      public async Task<Response<UserDto>> GetUserByNameAsync(string userName)
      {
         var user = await _userManager.FindByNameAsync(userName);

         if (user == null) return Response<UserDto>.Fail(new ErrorDto("User not found", true), 404);

         return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
      }
   }
}
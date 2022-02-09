using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AutoMapper;

namespace AuthServer.Service.Mapper
{
   public class MapperProfile : Profile
   {
      public MapperProfile()
      {
         CreateMap<User, UserDto>().ReverseMap();
         CreateMap<Product, ProductDto>().ReverseMap();
      }
   }
}
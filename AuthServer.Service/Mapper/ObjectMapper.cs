using AutoMapper;

namespace AuthServer.Service.Mapper
{
   public static class ObjectMapper
   {
      private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
      {
         var config = new MapperConfiguration(cfg =>
          {
             cfg.AddProfile<MapperProfile>();
          });

         return config.CreateMapper();
      });
      public static IMapper Mapper => Lazy.Value;
   }
}
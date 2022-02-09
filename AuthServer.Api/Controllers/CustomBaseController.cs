using Microsoft.AspNetCore.Mvc;
using Shared.Dto;

namespace AuthServer.Api.Controllers
{
   public class CustomBaseController : ControllerBase
   {
      public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
      {
         return new ObjectResult(response)
         {
            StatusCode = (int)response.StatusCode
         };
      }
   }
}
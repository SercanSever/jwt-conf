using System.Text.Json.Serialization;

namespace Shared.Dto
{
   public class Response<T> where T : class
   {
      public T? Data { get; private set; }
      public int StatusCode { get; private set; }

      [JsonIgnore]
      public bool IsSuccessful { get; private set; }
      public ErrorDto? Error { get; private set; }
      public static Response<T> Success(T data, int statusCode)
      {
         return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
      }
      public static Response<T> Success(int statusCode)
      {
         return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
      }
      public static Response<T> Fail(ErrorDto error, int statusCode)
      {
         return new Response<T> { Error = error, StatusCode = statusCode, IsSuccessful = false };
      }
      public static Response<T> Fail(string errorMessage, bool isShown, int statusCode)
      {
         var errorDto = new ErrorDto(errorMessage, isShown);

         return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
      }
   }
}
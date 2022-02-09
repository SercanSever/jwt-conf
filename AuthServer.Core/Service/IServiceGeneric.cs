using System.Linq.Expressions;
using Shared.Dto;

namespace AuthServer.Core.Service
{
   public interface IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
   {
      Task<Response<TDto>> GetByIdAsync(int id);
      Task<Response<List<TDto>>> GetAllAsync();
      Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
      Task<Response<TDto>> AddAsync(TDto entity);
      Task<Response<NoDataDto>> Remove(int id);
      Task<Response<NoDataDto>> UpdateAsync(TDto entity, int id);
   }
}
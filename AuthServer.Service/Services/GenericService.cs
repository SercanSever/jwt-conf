using System.Linq.Expressions;
using AuthServer.Core.Repository;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mapper;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace AuthServer.Service.Services
{
   public class GenericService<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IGenericRepository<TEntity> _repository;

      public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
      {
         _unitOfWork = unitOfWork;
         _repository = repository;
      }

      public async Task<Response<TDto>> AddAsync(TDto entity)
      {
         var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
         await _repository.AddAsync(newEntity);
         await _unitOfWork.CommitAsync();
         var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
         return Response<TDto>.Success(newDto, 200);
      }

      public async Task<Response<List<TDto>>> GetAllAsync()
      {
         var products = ObjectMapper.Mapper.Map<List<TDto>>(await _repository.GetAllAsync());
         return Response<List<TDto>>.Success(products, 200);
      }

      public async Task<Response<TDto>> GetByIdAsync(int id)
      {
         var product = await _repository.GetByIdAsync(id);
         if (product == null)
         {
            return Response<TDto>.Fail("ID not found", true, 404);
         }
         return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);
      }

      public async Task<Response<NoDataDto>> Remove(int id)
      {
         var isExistEntity = await _repository.GetByIdAsync(id);
         if (isExistEntity == null)
         {
            return Response<NoDataDto>.Fail("ID not found", true, 404);
         }
         _repository.Remove(isExistEntity);
         await _unitOfWork.CommitAsync();
         return Response<NoDataDto>.Success(204);
      }

      public async Task<Response<NoDataDto>> UpdateAsync(TDto entity, int id)
      {
         var isExistEntity = await _repository.GetByIdAsync(id);
         if (isExistEntity == null)
         {
            return Response<NoDataDto>.Fail("ID not found", true, 404);
         }
         var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
         _repository.Update(newEntity);
         await _unitOfWork.CommitAsync();
         return Response<NoDataDto>.Success(204);
      }

      public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
      {
         var list = _repository.Where(predicate);
         return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
      }
   }
}
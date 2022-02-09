using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ProductController : CustomBaseController
   {
      private readonly IServiceGeneric<Product, ProductDto> _productService;

      public ProductController(IServiceGeneric<Product, ProductDto> productService)
      {
         _productService = productService;
      }
      [HttpGet("getall")]
      public async Task<IActionResult> GetAll()
      {
         var result = await _productService.GetAllAsync();
         return ActionResultInstance(result);
      }
      [HttpGet("getbyid/{id}")]
      public async Task<IActionResult> GetById(int id)
      {
         var result = await _productService.GetByIdAsync(id);
         return ActionResultInstance(result);
      }
      [HttpPost("create")]
      public async Task<IActionResult> Create(ProductDto productDto)
      {
         var result = await _productService.AddAsync(productDto);
         return ActionResultInstance(result);
      }
      [HttpPut("update")]
      public async Task<IActionResult> Update(ProductDto productDto)
      {
         var result = await _productService.UpdateAsync(productDto, productDto.Id);
         return ActionResultInstance(result);
      }
      [HttpDelete("delete/{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         var result = await _productService.Remove(id);
         return ActionResultInstance(result);
      }
   }
}
using eCommerceWebMVC.Models.DTOs.ProductDTOs.Request;
using eCommerceWebMVC.Models.DTOs.ProductDTOs.Response;
using MicroserviceMVC.Common.Handler;

namespace eCommerceWebMVC.Services.ProductServices.Interface
{
    public interface IProductService
    {
        Task<Result<ProductResponseDto>> CreaAsync(ProductRequestDto model);
        Task<Result<IEnumerable<ProductResponseDto>>> GetAllAsync();
        Task<Result<ProductResponseDto>> GetByIdAsync(int id);
        Task<Result<ProductResponseDto>> UpdateAsync(int id, ProductRequestDto model);
        Task<Result<bool>> DeleteAsync(int id);
    }
}

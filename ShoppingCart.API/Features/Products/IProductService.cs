using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.DTOs.ProductDTOs;

namespace ShoppingCart.API.Features.Products
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductResponseDto>>> GetAllAsync();
    }
}

using SharedKernel.Results;
using Order.API.Features.Products.Dtos.Response;
namespace Order.API.Features.Products.Services;

public interface IProductService
{
    Task<Result<IEnumerable<ProductResponseDto>>> GetAllAsync();
}

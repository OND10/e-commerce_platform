using MediatR;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Queries.GetProducts
{
    public class GetProductQuery : IRequest<Result<IEnumerable<ProductResponseDto>>>
    {
    }
}

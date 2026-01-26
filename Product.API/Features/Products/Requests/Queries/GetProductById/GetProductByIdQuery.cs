using MediatR;
using Common.BuildingBlocks.Results;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Result<ProductResponseDto>>
    {
        public int Id { get; set; }
    }
}

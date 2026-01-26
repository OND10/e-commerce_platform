using MediatR;
using Common.BuildingBlocks.Results;
using Product.API.Features.Products.DTOs;
using System.Collections.Generic;

namespace Product.API.Features.Products.Requests.Queries.GetProducts
{
    public class GetProductQuery : IRequest<Result<IEnumerable<ProductResponseDto>>>
    {
    }
}

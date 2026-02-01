using SharedKernels.Abstractions.Messaging;
using SharedKernels.Results;
using Product.API.Features.Products.DTOs;
using System.Collections.Generic;

namespace Product.API.Features.Products.Requests.Queries.GetProducts
{
    public class GetProductQuery : IQuery<IEnumerable<ProductResponseDto>>
    {
    }
}

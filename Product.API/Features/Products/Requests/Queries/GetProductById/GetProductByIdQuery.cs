using SharedKernel.Abstractions.Messaging;
using SharedKernel.Results;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Queries.GetProductById
{
    public class GetProductByIdQuery : IQuery<ProductResponseDto>
    {
        public int Id { get; set; }
    }
}

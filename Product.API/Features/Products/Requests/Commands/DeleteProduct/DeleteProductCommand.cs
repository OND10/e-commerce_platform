using MediatR;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}

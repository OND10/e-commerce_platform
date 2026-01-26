using MediatR;
using Common.BuildingBlocks.Results;

namespace Product.API.Features.Products.Requests.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}

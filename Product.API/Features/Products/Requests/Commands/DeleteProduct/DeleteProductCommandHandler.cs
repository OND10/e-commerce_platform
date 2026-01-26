using MediatR;
using Common.BuildingBlocks.Results;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
             var result = await _productRepository.DeleteAsync(request.Id);
             if (result)
             {
                 return Result.Success(true);
             }
             return Result.Failure<bool>("Product not found or delete failed");
        }
    }
}

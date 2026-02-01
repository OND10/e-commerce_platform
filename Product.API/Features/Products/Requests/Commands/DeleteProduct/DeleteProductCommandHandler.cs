using SharedKernel.Abstractions.Messaging;
using SharedKernel.Results;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
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
             return Result.Failure<bool>(Error.NotFound("Product.NotFound", "Product not found or delete failed"));
        }
    }
}

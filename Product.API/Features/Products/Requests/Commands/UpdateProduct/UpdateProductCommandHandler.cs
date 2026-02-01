using MediatR;
using SharedKernels.Results;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;
using AutoMapper;

namespace Product.API.Features.Products.Requests.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductResponseDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(request.Id);
            if (productToUpdate == null)
            {
                return Result.Failure<ProductResponseDto>(Error.NotFound("Product.NotFound", "Product not found"));
            }

            // Manually mapping or using Mapper if configured for DTO -> Entity update?
            // Assuming simple property update
            // Use domain method to update product
            var updateResult = productToUpdate.Update(
                name: request.Name,
                description: request.Description,
                price: (decimal)request.Price,
                stock: request.NumberofProduct,
                category: request.Category,
                imageUrl: request.ImageUrl
            );

            if (updateResult.IsFailure)
            {
                return Result.Failure<ProductResponseDto>(updateResult.Error);
            }

            var updatedProduct = await _productRepository.UpdateAsync(productToUpdate);
            var response = _mapper.Map<ProductResponseDto>(updatedProduct);
            
            return Result.Success(response);
        }
    }
}

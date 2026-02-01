using SharedKernel.Abstractions.Messaging;
using SharedKernel.Results;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;
using AutoMapper;

namespace Product.API.Features.Products.Requests.Commands.AddProduct
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductResponseDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            // Use factory method to create product with validation
            var productResult = Entities.Product.Create(
                name: request.Name,
                description: request.Description,
                price: (decimal)request.Price,
                stock: request.NumberofProduct,
                category: request.Category,
                imageUrl: request.ImageUrl
            );

            if (productResult.IsFailure)
                return Result.Failure<ProductResponseDto>(productResult.Error);

            // Save to repository
            var createdProduct = await _productRepository.CreateAsync(productResult.Value);
            
            // Map to response DTO
            var response = _mapper.Map<ProductResponseDto>(createdProduct);
            
            return Result.Success(response);
        }
    }
}

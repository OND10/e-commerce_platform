using MediatR;
using Common.BuildingBlocks.Results;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;
using AutoMapper;

namespace Product.API.Features.Products.Requests.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result<ProductResponseDto>>
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
            var product = new Entities.Product
            {
                Name = request.Name,
                Description = request.Description,
                NumberofProduct = request.NumberofProduct,
                Category = request.Category,
                Price = request.Price,
                ImageUrl = request.ImageUrl
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            var response = _mapper.Map<ProductResponseDto>(createdProduct);
            
            return Result.Success(response);
        }
    }
}

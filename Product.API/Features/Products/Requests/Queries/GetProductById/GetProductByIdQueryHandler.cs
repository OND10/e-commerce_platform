using MediatR;
using Common.BuildingBlocks.Results;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;
using AutoMapper;

namespace Product.API.Features.Products.Requests.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
    {
         private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                 return Result.Failure<ProductResponseDto>("Product not found");
            }
            var response = _mapper.Map<ProductResponseDto>(product);
            return Result.Success(response);
        }
    }
}

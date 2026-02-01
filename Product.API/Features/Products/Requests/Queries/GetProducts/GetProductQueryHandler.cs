using SharedKernel.Abstractions.Messaging;
using SharedKernel.Results;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;
using AutoMapper;
using System.Collections.Generic;

namespace Product.API.Features.Products.Requests.Queries.GetProducts
{
    public class GetProductQueryHandler : IQueryHandler<GetProductQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<ProductResponseDto>>(products);
            return Result.Success(response);
        }
    }
}

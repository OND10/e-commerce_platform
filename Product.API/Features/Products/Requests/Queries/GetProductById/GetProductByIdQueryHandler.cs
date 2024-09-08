using MediatR;
using OnMapper;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
    {

        private readonly IProductRepository _repository;
        private readonly OnMapping _mapper;
        public GetProductByIdQueryHandler(IProductRepository repository, OnMapping mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }
        public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdAsync(request.Id);
            var mappedResult = await _mapper.Map<Entities.Product, ProductResponseDto>(result);

            return await Result<ProductResponseDto>.SuccessAsync(mappedResult.Data, "Viewed Successfully", true);
        }
    }
}

using MediatR;
using OnMapper;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Queries.GetProducts
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IProductRepository _repository;
        private readonly OnMapping _mapper;
        public GetProductQueryHandler(IProductRepository repository, OnMapping mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }
        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync();
            var mappedResult = await _mapper.MapCollection<Entities.Product, ProductResponseDto>(result);

            return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(mappedResult.Data, "Viewed Successfully", true);
        }
    }
}

using MediatR;
using OnMapper;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result<ProductResponseDto>>
    {

        private readonly IProductRepository _repository;
        private readonly IUnitofWork _unitofWork;
        private readonly OnMapping _mapper;
        public AddProductCommandHandler(IProductRepository repository, IUnitofWork unitofWork, OnMapping mapper)
        {
            _repository = repository;
            _unitofWork = unitofWork;
            _mapper = mapper;

        }

        public async Task<Result<ProductResponseDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var model = await _mapper.Map<AddProductCommand, Product.API.Entities.Product>(request);
            var result = await _repository.CreateAsync(model.Data);
            await _unitofWork.SaveChangesAsync();
            var mappedResult = await _mapper.Map<Product.API.Entities.Product, ProductResponseDto>(result);

            return await Result<ProductResponseDto>.SuccessAsync(mappedResult.Data, "Viewed Successfully", true);
        }
    }
}

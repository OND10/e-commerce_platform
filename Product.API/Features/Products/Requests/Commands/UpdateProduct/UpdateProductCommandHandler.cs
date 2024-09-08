using MediatR;
using OnMapper;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductResponseDto>>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitofWork _unitofWork;
        private readonly OnMapping _mapper;
        public UpdateProductCommandHandler(IProductRepository repository, IUnitofWork unitofWork, OnMapping mapper)
        {
            _repository = repository;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        public async Task<Result<ProductResponseDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var model = await _mapper.Map<UpdateProductCommand, Entities.Product>(request);
            var result = await _repository.UpdateAsync(model.Data);
            await _unitofWork.SaveChangesAsync();
            var mappedResult = await _mapper.Map<Entities.Product, ProductResponseDto>(result);

            return await Result<ProductResponseDto>.SuccessAsync(mappedResult.Data, "Viewed Successfully", true);
        }
    }
}

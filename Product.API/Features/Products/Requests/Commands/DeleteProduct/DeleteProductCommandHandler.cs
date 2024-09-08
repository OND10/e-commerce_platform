using MediatR;
using Product.API.Common.Handler;
using Product.API.Features.Products.Repository.Interface;

namespace Product.API.Features.Products.Requests.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
    {

        private readonly IProductRepository _repository;
        private readonly IUnitofWork _unitofWork;
        public DeleteProductCommandHandler(IProductRepository repository, IUnitofWork unitofWork)
        {
            _repository = repository;
            _unitofWork = unitofWork;

        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {

            var result = await _repository.DeleteAsync(request.Id);
            await _unitofWork.SaveChangesAsync();
            return await Result<bool>.SuccessAsync(true, "Deleted Successfully", true);
        }
    }
}

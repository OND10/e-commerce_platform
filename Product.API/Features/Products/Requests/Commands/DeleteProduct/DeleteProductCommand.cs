using SharedKernel.Abstractions.Messaging;
using SharedKernel.Results;

namespace Product.API.Features.Products.Requests.Commands.DeleteProduct
{
    public class DeleteProductCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
}

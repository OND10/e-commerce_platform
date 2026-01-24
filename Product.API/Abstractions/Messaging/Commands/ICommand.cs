using MediatR;
using Product.API.Common.Handler;

namespace Product.API.Abstractions.Messaging.Commands
{
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}

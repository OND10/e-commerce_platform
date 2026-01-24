using MediatR;
using Product.API.Common.Handler;

namespace Product.API.Abstractions.Messaging.Commands
{
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where
        TCommand : ICommand<TResponse>
    {
    }
}

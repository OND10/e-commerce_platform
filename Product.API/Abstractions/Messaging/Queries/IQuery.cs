using MediatR;
using Product.API.Common.Handler;

namespace Product.API.Abstractions.Messaging.Queries
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}

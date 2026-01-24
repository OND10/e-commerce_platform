using MediatR;
using Product.API.Common.Handler;

namespace Product.API.Abstractions.Messaging.Queries
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}

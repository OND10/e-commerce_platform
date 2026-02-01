using MediatR;
using SharedKernels.Results;

namespace SharedKernels.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
using MediatR;
using SharedKernels.Abstractions.Messaging;
using SharedKernels.Results;

namespace Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int orderId, string newStatus) : ICommand<bool>;
}

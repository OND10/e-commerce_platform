using MediatR;
using Common.BuildingBlocks.Results;

namespace Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int orderId, string newStatus) : IRequest<Result<bool>>;
}

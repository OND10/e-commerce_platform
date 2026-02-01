using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Dtos.Request;
using SharedKernels.Abstractions.Messaging;
using SharedKernels.Results;

namespace Order.API.Features.Orders.Requests.Commands.AddOrder
{
    public record AddOrderCommand(CartDto cartDto) : ICommand<OrderHeaderResponseDto>;
  
}

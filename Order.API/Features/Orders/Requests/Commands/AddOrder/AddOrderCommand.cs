using MediatR;
using SharedKernel.Results;
using Order.API.Features.Orders.Dtos.Request;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Dtos.Request; 

namespace Order.API.Features.Orders.Requests.Commands.AddOrder
{
    public record AddOrderCommand(CartDto cartDto) : IRequest<Result<OrderHeaderResponseDto>>;
}

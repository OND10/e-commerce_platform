using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Common.Handler;
using Order.API.Features.Dtos.Request;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Orders.Requests.Commands.AddOrder
{
    public class AddOrderCommand : IRequest<Result<OrderHeaderResponseDto>>
    {
        public CartDto cartDto { get; set; }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.BuildingBlocks.Results; // Correct Result Namespace
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Orders.Requests.Commands.AddOrder;
using Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus;
using Order.API.Features.Orders.Requests.Queries.GetOrder;
using Order.API.Features.Orders.Requests.Queries.GetOrderById;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Features.Stripe.Requests.Commands.CreateStripeSession;
using Order.API.Features.Stripe.Requests.Queries.ValidateStripeSession;
// using Order.API.Common.Handler; // REMOVED

using Order.API.Features.Orders.Dtos.Request;
using Order.API.Features.Dtos.Request;

namespace Order.API.Features.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender _sender;
        public OrderController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public async Task<Result<IEnumerable<OrderHeaderResponseDto>>> Get(string? userId = "", CancellationToken cancellationToken = default)
        {
            if (User.IsInRole("Admin"))
            {
                var query = new GetOrderQuery { Role = "Admin" };
                return await _sender.Send(query, cancellationToken);
            }
            else
            {
                var query = new GetOrderQuery { Role = "CUSTOMER", userId = userId };
                return await _sender.Send(query, cancellationToken);
            }
        }

        [HttpGet("GetOrder/{id:int}")]
        public async Task<Result<OrderHeaderResponseDto>> Get(int id, CancellationToken cancellationToken)
        {
            var query = new GetOrderQueryById { orderId = id };
            return await _sender.Send(query, cancellationToken);
        }

        [HttpPost("CreateOrder")]
        public async Task<Result<OrderHeaderResponseDto>> Post(CartDto cartDto, CancellationToken cancellationToken)
        {
            var command = new AddOrderCommand(cartDto);
            return await _sender.Send(command, cancellationToken);
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<Result<StripeRequestDto>> CreateStripeSession([FromBody] StripeRequestDto request, CancellationToken cancellationToken)
        {
            var command = new CreateStripeSessionCommand
            {
                ApprovedUrl = request.ApprovedUrl,
                StripeSessionId = request.StripeSessionId,
                StripeSessionUrl = request.StripeSessionUrl,
                CancelUrl = request.CancelUrl,
                OrderHeader = request.OrderHeader,
            };
            return await _sender.Send(command, cancellationToken);
        }

        [HttpPost("ValidateStripeSession")]
        public async Task<Result<OrderHeaderResponseDto>> ValidateStripeSession([FromBody] int orderHeaderId, CancellationToken cancellationToken)
        {
            var query = new ValidateStripeSessionQuery(orderHeaderId);
            return await _sender.Send(query, cancellationToken);
        }

        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<Result<bool>> UpdateOrderStatus(int orderId, [FromBody] string newStatus, CancellationToken cancellationToken)
        {
            var command = new UpdateOrderStatusCommand(orderId, newStatus);
            return await _sender.Send(command, cancellationToken);
        }
    }
}

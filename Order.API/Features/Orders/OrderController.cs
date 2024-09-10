using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Common.Handler;
using Order.API.Features.Dtos.Request;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Orders.Requests.Commands.AddOrder;
using Order.API.Features.Orders.Requests.Commands.UpdateOrderStatus;
using Order.API.Features.Orders.Requests.Queries.GetOrder;
using Order.API.Features.Orders.Requests.Queries.GetOrderById;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Features.Stripe.Requests.Commands.CreateStripeSession;
using Order.API.Features.Stripe.Requests.Queries.ValidateStripeSession;
using Order.API.Shared;
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
        public async Task<Result<IEnumerable<OrderHeaderResponseDto>>>Get(string? userId = "", CancellationToken cancellationToken = default)
        {
            if(User.IsInRole("Admin"))
            {
                var query = new GetOrderQuery
                {
                    Role = "Admin"
                };

                var response = await _sender.Send(query, cancellationToken);

                if (response.IsSuccess)
                {
                    return await Result<IEnumerable<OrderHeaderResponseDto>>.SuccessAsync(response.Data, "GetAll order Successfully", true);
                }
                return await Result<IEnumerable<OrderHeaderResponseDto>>.FaildAsync(false, "Operation faild");

            }
            else
            {
                var query = new GetOrderQuery
                {
                    Role = "User",
                    userId = userId
                };

                var response = await _sender.Send(query, cancellationToken);
               
                if (response.IsSuccess)
                {
                    return await Result<IEnumerable<OrderHeaderResponseDto>>.SuccessAsync(response.Data, "GetAll user order Successfully", true);
                }
                return await Result<IEnumerable<OrderHeaderResponseDto>>.FaildAsync(false, "Operation faild");

            }
        }

        [HttpGet("GetOrder/{id:int}")]
        public async Task<Result<OrderHeaderResponseDto>> Get(int id, CancellationToken cancellationToken)
        {
            var query = new GetOrderQueryById
            {
                orderId = id
            };

            var response = await _sender.Send(query, cancellationToken);

            if (response.IsSuccess)
            {

                return await Result<OrderHeaderResponseDto>.SuccessAsync(response.Data, ResponseStatus.GetSuccess, true);
            }

            return await Result<OrderHeaderResponseDto>.FaildAsync(false, ResponseStatus.Faild);
        }

        [HttpPost("CreateOrder")]
        public async Task<Result<OrderHeaderResponseDto>> Post(CartDto cartDto, CancellationToken cancellationToken)
        {
            var command = new AddOrderCommand
            {
                cartDto = cartDto
            };

            var response = await _sender.Send(command, cancellationToken);
            if (response.IsSuccess)
            {
                return await Result<OrderHeaderResponseDto>.SuccessAsync(response.Data, ResponseStatus.CreatSuccess, true);
            }

            return await Result<OrderHeaderResponseDto>.FaildAsync(false, ResponseStatus.Faild);
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

            var response = await _sender.Send(command, cancellationToken);

            if(response.IsSuccess)
            {
                return await Result<StripeRequestDto>.SuccessAsync(response.Data, $"SessionId is {ResponseStatus.CreatSuccess}", true);
            }

            return await Result<StripeRequestDto>.FaildAsync(false, ResponseStatus.Faild);
        }

        [HttpPost("ValidateStripeSession")]
        public async Task<Result<OrderHeaderResponseDto>> ValidateStripeSession([FromBody] int orderHeaderId, CancellationToken cancellationToken)
        {
            var query = new ValidateStripeSessionQuery
            {
                OrderHeadreId = orderHeaderId
            };

            var response = await _sender.Send(query, cancellationToken);

            if(response.IsSuccess)
            {
                return await Result<OrderHeaderResponseDto>.SuccessAsync(response.Data, "Order is Verfied Successfully", true);
            }

            return await Result<OrderHeaderResponseDto>.FaildAsync(false, "Confirmation order is Faild");
        }

        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<Result<bool>>UpdateOrderStatus(int orderId, [FromBody] string newStatus, CancellationToken cancellationToken)
        {
            var command = new UpdateOrderStatusCommand
            {
                orderId = orderId,
                newStatus = newStatus
            };

            var response = await _sender.Send(command, cancellationToken);

            if (response.IsSuccess)
            {
                return await Result<bool>.SuccessAsync(true, "OrderStatus is updated Successfully", true);
            }


            return await Result<bool>.FaildAsync(false, "OrderStatus is not updated");
        }
    }
}

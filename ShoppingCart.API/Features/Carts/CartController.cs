using MediatR;
using MessageBus.Services;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Features.Carts.Requests.Command.AddCart;
using ShoppingCart.API.Features.Carts.Requests.Command.ApplyCartCoupon;
using ShoppingCart.API.Features.Carts.Requests.Command.DeleteCart;
using ShoppingCart.API.Features.Carts.Requests.Command.RemoveCartCoupon;
using ShoppingCart.API.Features.Carts.Requests.Queries.GetCarts;
using ShoppingCart.API.Features.DTOs.CartDTOs;

namespace ShoppingCart.API.Features.Carts
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMessageBusService _messageBus;
        private readonly IConfiguration _configuration;
        public CartController(ISender sender, IMessageBusService messageBus, IConfiguration configuration)
        {
            _sender = sender;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpGet("{userId}")]
        public async Task<Result<IEnumerable<CartDto>>> Get(string userId, CancellationToken cancellationToken)
        {
            var query = new GetCartsQuery
            {
                UserId = userId
            };

            var result = await _sender.Send(query, cancellationToken);
            if (result != null && result.IsSuccess)
            {
                return await Result<IEnumerable<CartDto>>.SuccessAsync(result.Data, "Viewed Successfully", true);
            }

            return await Result<IEnumerable<CartDto>>.FaildAsync(false, "Not Viewed Successfully");
        }

        [HttpPost("ApplyCoupon")]
        public async Task<Result<bool>> ApplyCoupon([FromBody] CartDto cart, CancellationToken cancellationToken)
        {
            var command = new ApplyCartCouponCommand
            {
                CartDetailsResponse = cart.CartDetailsResponse,
                CartHeaderResponse = cart.CartHeaderResponse,
            };

            var result = await _sender.Send(command, cancellationToken);
            if (result.IsSuccess)
            {

                return await Result<bool>.SuccessAsync(true, "Applied Successfully", true);
            }

            return await Result<bool>.FaildAsync(false, "Applied Successfully");
        }

        [HttpPost("RemoveCoupon")]
        public async Task<Result<bool>> RemoveCoupon([FromBody] CartDto cart, CancellationToken cancellationToken)
        {
            var command = new RemoveCartCouponCommand
            {
                CartDetailsResponse = cart.CartDetailsResponse,
                CartHeaderResponse = cart.CartHeaderResponse,
            };

            var result = await _sender.Send(command, cancellationToken);
            if (result.IsSuccess)
            {

                return await Result<bool>.SuccessAsync(true, "Applied Successfully", true);
            }

            return await Result<bool>.FaildAsync(false, "Applied Successfully");
        }


        [HttpPost("createCart")]
        public async Task<Result<CartDto>> Post(CartDto cartDto, CancellationToken cancellationToken)
        {
            var command = new AddCartCommand()
            {
                CartDetailsResponse = cartDto.CartDetailsResponse,
                CartHeaderResponse = cartDto.CartHeaderResponse
            };

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                return await Result<CartDto>.SuccessAsync(result.Data, "Added Successfuly", true);
            }

            return await Result<CartDto>.FaildAsync(false, "Not added");
        }

        [HttpPost("removeCart")]
        public async Task<Result<bool>> Delete([FromBody] int cartDetailsId, CancellationToken cancellationToken)
        {
            var command = new DeleteCartCommand()
            {
                cartDetailsId = cartDetailsId
            };

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                return await Result<bool>.SuccessAsync(true, "Deleted Successfuly", true);
            }

            return await Result<bool>.FaildAsync(false, "Not Deleted");
        }

        [HttpPost("EmailCartRequest")]
        public async Task<Result<bool>> EmailCartRequest([FromBody] CartDto cart, CancellationToken cancellationToken)
        {
            try
            {
                await _messageBus.PublishMessage(cart, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
                return await Result<bool>.SuccessAsync(true, "Message is sent Successfully using Service Bus", true, cancellationToken);
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Message not sent using service Bus{ex.Message}");

            }
        }

    }
}

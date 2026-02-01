using MediatR;
using SharedKernels.Messaging; // Use IEventBus
using SharedKernels.Results;
using Microsoft.EntityFrameworkCore;
using Order.API.Entities;
using Order.API.DataBase;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Rewards;
using Stripe;
using Stripe.Checkout;

namespace Order.API.Features.Stripe.Requests.Queries.ValidateStripeSession
{
    public class ValidateStripeSessionQueryHandler : IRequestHandler<ValidateStripeSessionQuery, Result<OrderHeaderResponseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;

        public ValidateStripeSessionQueryHandler(AppDbContext context, IEventBus eventBus, IConfiguration configuration)
        {
            _context = context;
            _eventBus = eventBus;
            _configuration = configuration;
        }

        public async Task<Result<OrderHeaderResponseDto>> Handle(ValidateStripeSessionQuery request, CancellationToken cancellationToken)
        {
            OrderHeader orderHeader = await _context.OrderHeaders.FirstAsync(u => u.Id == request.OrderHeadreId, cancellationToken);

            var serivce = new SessionService();
            Session checkSessionId = await serivce.GetAsync(orderHeader.StripeSessionId, cancellationToken: cancellationToken);

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent = await paymentIntentService.GetAsync(checkSessionId.PaymentIntentId, cancellationToken: cancellationToken);

            if (paymentIntent.Status == "succeeded")
            {
                orderHeader.SetPaymentIntent(paymentIntent.Id, orderHeader.StripeSessionId);
                // orderHeader.Status = StatusEnum.Status_Approved; // Use state machine or manual update if enum conversion needed
                // Assuming we can set state directly for now or via method
                // mapped to OrderState? Approved means Paid?
                orderHeader.SetState(OrderState.Paid); 
                
                await _context.SaveChangesAsync(cancellationToken);

                RewardDto rewardDto = new RewardDto()
                {
                    OrderId = orderHeader.Id,
                    UserId = orderHeader.UserId,
                    RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal)
                };

                // MassTransit Publish (no topic name needed if using Type-based routing, typically)
                // But keeping user logic if using topic name config? MassTransit standard is publish T.
                await _eventBus.PublishAsync(rewardDto, cancellationToken);
            }

            var orderHeaderResponse = new OrderHeaderResponseDto()
            {
                Id = orderHeader.Id,
                UserId = orderHeader.UserId,
                CouponCode = orderHeader.CouponCode,
                Discount = orderHeader.Discount,
                OrderTotal = orderHeader.OrderTotal,
                Name = orderHeader.Name,
                Email = orderHeader.EmailAddress,
                PhoneNumber = orderHeader.PhoneNumber,
                Status = orderHeader.OrderState.ToString(),
                OrderTime = orderHeader.OrderTime,
                OrderDetails = new List<OrderDetailsResponseDto>()
            };

            return Result.Success(orderHeaderResponse);
        }
    }
}

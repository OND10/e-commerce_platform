using MediatR;
using MessageBus.Services;
using Microsoft.EntityFrameworkCore;
using Order.API.Common.Enum;
using Order.API.Common.Handler;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Rewards;
using Stripe;
using Stripe.Checkout;

namespace Order.API.Features.Stripe.Requests.Queries.ValidateStripeSession
{
    public class ValidateStripeSessionQueryHandler : IRequestHandler<ValidateStripeSessionQuery, Result<OrderHeaderResponseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMessageBusService _messageBusService;
        private readonly IConfiguration _configuration;
        public ValidateStripeSessionQueryHandler(AppDbContext context, IMessageBusService messageBusService, IConfiguration configuration)
        {
            _context = context;
            _messageBusService = messageBusService;
            _configuration = configuration;
        }
        public async Task<Result<OrderHeaderResponseDto>> Handle(ValidateStripeSessionQuery request, CancellationToken cancellationToken)
        {
            OrderHeader orderHeader = await _context.OrderHeaders.FirstAsync(u => u.Id == request.OrderHeadreId);

            var serivce = new SessionService();

            //looking for StripeSessionIs in Stripe

            Session checkSessionId = serivce.Get(orderHeader.StripeSessionId);

            //Check Order Status
            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent = paymentIntentService.Get(checkSessionId.PaymentIntentId);

            if(paymentIntent.Status == "succeeded")
            {
                orderHeader.PaymentIntentId = paymentIntent.Id;
                orderHeader.Status = StatusEnum.Status_Approved;
                _context.SaveChanges();

                RewardDto rewardDto = new RewardDto()
                {
                    OrderId = orderHeader.Id,
                    UserId = orderHeader.UserId,
                    RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal)
                };

                string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                await _messageBusService.PublishMessage(rewardDto, topicName);
            }

            var orderHeaderResponse = new OrderHeaderResponseDto()
            {
                UserId = orderHeader.UserId,
                CouponCode = orderHeader.CouponCode,
                Discount = orderHeader.Discount,
                OrderTotal = orderHeader.OrderTotal,
                Name = orderHeader.Name,
                Email = orderHeader.Email,
                PhoneNumber = orderHeader.PhoneNumber,
                Status = orderHeader.Status,
                OrderTime = orderHeader.OrderTime,
                OrderDetails = new List<OrderDetailsResponseDto>()
            };


            return await Result<OrderHeaderResponseDto>.SuccessAsync(orderHeaderResponse, "Order is Verfied Successfully", true);
        }
    }
}

using MediatR;
using Order.API.Common.Handler;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Shared;
using Stripe;
using Stripe.Checkout;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace Order.API.Features.Stripe.Requests.Commands.CreateStripeSession
{
    public class CreateStripeSessionCommandHandler : IRequestHandler<CreateStripeSessionCommand, Result<StripeRequestDto>>
    {
        private readonly AppDbContext _context;
        public CreateStripeSessionCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<StripeRequestDto>> Handle(CreateStripeSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = request.ApprovedUrl,
                    CancelUrl = request.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                var couponCode = request.OrderHeader.CouponCode;
                Console.WriteLine($"Coupon code being used: {couponCode}");

                if (request.OrderHeader.Discount > 0 && !string.IsNullOrEmpty(couponCode))
                {
                    var stripeCouponService = new CouponService();
                    try
                    {
                        // List coupons to verify existence and ID
                        var coupons = await stripeCouponService.ListAsync();
                        var coupon = coupons.Data.FirstOrDefault(c => c.Name == couponCode);
                        if (coupon == null)
                        {
                            return await Result<StripeRequestDto>.FaildAsync(false, $"Coupon {couponCode} does not exist.");
                        }
                        Console.WriteLine($"Stripe Coupon retrieved: {coupon.Id}, {coupon.PercentOff}% off");

                        var discountObj = new List<SessionDiscountOptions>
                    {
                        new SessionDiscountOptions
                        {
                            Coupon = coupon.Id
                        }
                    };
                        options.Discounts = discountObj;
                    }
                    catch (StripeException stripeEx)
                    {
                        Console.WriteLine($"Stripe error: {stripeEx.Message}");
                        return await Result<StripeRequestDto>.FaildAsync(false, $"Coupon validation failed: {stripeEx.Message}");
                    }
                }

                foreach (var item in request.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                // Creating a new session
                Session session = service.Create(options);
                request.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _context.OrderHeaders.First(u => u.Id == request.OrderHeader.Id);
                orderHeader.StripeSessionId = session.Id;
                _context.SaveChanges();

                var mappedResponse = new StripeRequestDto
                {
                    StripeSessionId = request.StripeSessionId,
                    StripeSessionUrl = request.StripeSessionUrl,
                    ApprovedUrl = request.ApprovedUrl,
                    CancelUrl = request.CancelUrl,
                    OrderHeader = request.OrderHeader
                };

                return await Result<StripeRequestDto>.SuccessAsync(mappedResponse, $"SessionId is {ResponseStatus.CreatSuccess}", true);
            }
            catch (Exception ex)
            {
                return await Result<StripeRequestDto>.FaildAsync(false, ex.Message);
            }

        }
    }

}

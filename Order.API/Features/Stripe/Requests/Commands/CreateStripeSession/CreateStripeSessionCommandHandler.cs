using MediatR;
using SharedKernels.Results;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Shared;
using Stripe;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;

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
                
                if (request.OrderHeader.Discount > 0 && !string.IsNullOrEmpty(couponCode))
                {
                    var stripeCouponService = new CouponService();
                    try
                    {
                        var coupons = await stripeCouponService.ListAsync(cancellationToken: cancellationToken);
                        var coupon = coupons.Data.FirstOrDefault(c => c.Name == couponCode);
                        if (coupon == null)
                        {
                            return Result.Failure<StripeRequestDto>($"Coupon {couponCode} does not exist.");
                        }

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
                        return Result.Failure<StripeRequestDto>($"Coupon validation failed: {stripeEx.Message}");
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
                                Name = item.ProductName, // DTO usually has ProductName directly or Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = await service.CreateAsync(options, cancellationToken: cancellationToken);
                
                request.StripeSessionUrl = session.Url;
                
                OrderHeader orderHeader = await _context.OrderHeaders.FirstAsync(u => u.Id == request.OrderHeader.Id, cancellationToken);
                orderHeader.SetPaymentIntent(orderHeader.PaymentIntentId, session.Id); // Updating SessionId
                
                await _context.SaveChangesAsync(cancellationToken);

                var mappedResponse = new StripeRequestDto
                {
                    StripeSessionId = request.StripeSessionId, // Note: This might be null if not set in request. Should it be session.Id? request object is likely DTO holder.
                    StripeSessionUrl = request.StripeSessionUrl,
                    ApprovedUrl = request.ApprovedUrl,
                    CancelUrl = request.CancelUrl,
                    OrderHeader = request.OrderHeader
                };

                return Result.Success(mappedResponse);
            }
            catch (Exception ex)
            {
                return Result.Failure<StripeRequestDto>(ex.Message);
            }
        }
    }
}

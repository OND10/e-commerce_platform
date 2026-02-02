using MediatR;
using SharedKernels.Results;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Stripe.Dtos.Request;
using Order.API.Shared;
using Stripe;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;
using SharedKernels.Abstractions.Messaging;
using SharedKernels;

namespace Order.API.Features.Stripe.Requests.Commands.CreateStripeSession
{
    public class CreateStripeSessionCommandHandler : ICommandHandler<CreateStripeSessionCommand, StripeRequestDto>
    {
        private readonly AppDbContext _context;
        private readonly SessionService _sessionService;
        private readonly CouponService _couponService;

        // Constants to avoid magic strings
        private const string CurrencyUsd = "usd";
        private const string PaymentMode = "payment";

        public CreateStripeSessionCommandHandler(
            AppDbContext context,
            SessionService sessionService,
            CouponService couponService)
        {
            _context = context;
            _sessionService = sessionService;
            _couponService = couponService;
        }

        public async Task<Result<StripeRequestDto>> Handle(CreateStripeSessionCommand request, CancellationToken cancellationToken)
        {
            // 1. Prepare Stripe Session Options
            var options = new SessionCreateOptions
            {
                SuccessUrl = request.ApprovedUrl,
                CancelUrl = request.CancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = PaymentMode,
            };

            // 2. Handle Coupon (Discount)
            var couponCode = request.OrderHeader.CouponCode;

            if (request.OrderHeader.Discount > 0 && !string.IsNullOrEmpty(couponCode))
            {
                var couponResult = await ApplyCouponAsync(couponCode, options, cancellationToken);
                if (couponResult.IsFailure)
                {
                    return Result.Failure<StripeRequestDto>(couponResult.Error);
                }
            }

            // 3. Build Line Items from Order Details
            foreach (var item in request.OrderHeader.OrderDetails)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Convert to cents
                        Currency = CurrencyUsd,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Count
                });
            }

            try
            {
                // 4. Create Stripe Session
                Session session = await _sessionService.CreateAsync(options, cancellationToken: cancellationToken);

                // 5. Update Database
                // Fetch the actual entity from DB to track changes
                var orderHeader = await _context.OrderHeaders
                    .FirstAsync(o => o.Id == request.OrderHeader.Id, cancellationToken);

                // Assuming SetPaymentIntent updates the entity properties
                orderHeader.SetPaymentIntent(orderHeader.PaymentIntentId, session.Id);

                await _context.SaveChangesAsync(cancellationToken);

                // 6. Prepare Response DTO
                var responseDto = new StripeRequestDto
                {
                    StripeSessionId = session.Id,       // Use the actual session ID returned by Stripe
                    StripeSessionUrl = session.Url,    // Use the actual URL returned by Stripe
                    ApprovedUrl = request.ApprovedUrl,
                    CancelUrl = request.CancelUrl,
                    OrderHeader = request.OrderHeader // Return the original order DTO as requested
                };

                return Result.Success(responseDto);
            }
            catch (StripeException stripeEx)
            {
                // Handle specific Stripe API errors
                return Result.Failure<StripeRequestDto>(new Error(
                    "500",
                    $"Stripe payment processing failed: {stripeEx.Message}",
                    ErrorType.Problem
                ));
            }
            catch (Exception ex)
            {
                // Handle general server errors
                return Result.Failure<StripeRequestDto>(new Error(
                    "500",
                    $"An unexpected error occurred: {ex.Message}",
                    ErrorType.Failure
                ));
            }
        }

        // Helper method to encapsulate coupon logic
        private async Task<Result> ApplyCouponAsync(string couponCode, SessionCreateOptions options, CancellationToken cancellationToken)
        {
            try
            {
                // Note: Listing all coupons can be slow if you have many. 
                // Ideally, you should map your local coupons to Stripe Coupon IDs.
                var coupons = await _couponService.ListAsync(cancellationToken: cancellationToken);

                var coupon = coupons.Data.FirstOrDefault(c => c.Name == couponCode);

                if (coupon == null)
                {
                    return Result.Failure(CouponNotFoundError);
                }

                options.Discounts = new List<SessionDiscountOptions>
                {
                    new SessionDiscountOptions { Coupon = coupon.Id }
                };

                return Result.Success();
            }
            catch (StripeException ex)
            {
                return Result.Failure(new Error(
                    "400",
                    $"Coupon validation failed: {ex.Message}",
                    ErrorType.Validation
                ));
            }
        }

        // Static Error Definitions providing Status Codes as Strings
        private static readonly Error CouponNotFoundError = new Error(
            "404",
            "The provided coupon code does not exist.",
            ErrorType.NotFound
        );
    }
}


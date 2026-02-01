using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using Order.API.DataBase;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Orders.Requests.Queries.GetOrderById
{
    public class GetOrderQueryByIdHandler : IRequestHandler<GetOrderQueryById, Result<OrderHeaderResponseDto>>
    {
        private readonly AppDbContext _context;

        public GetOrderQueryByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<OrderHeaderResponseDto>> Handle(GetOrderQueryById request, CancellationToken cancellationToken)
        {
            var orderHeader = await _context.OrderHeaders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == request.orderId, cancellationToken);

            if (orderHeader == null)
            {
                return Result.Failure<OrderHeaderResponseDto>("Order not found");
            }

            var orderDetailsList = orderHeader.OrderDetails.Select(item => new OrderDetailsResponseDto
            {
                Id = item.Id,
                Count = item.Count,
                OrderHeaderId = item.OrderHeaderId,
                Price = item.Price,
                Product = item.Product,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
            }).ToList();

            var orderResponse = new OrderHeaderResponseDto
            {
                Id = orderHeader.Id,
                CouponCode = orderHeader.CouponCode,
                Discount = orderHeader.Discount,
                Email = orderHeader.EmailAddress,
                Name = orderHeader.Name,
                OrderTime = orderHeader.OrderTime,
                OrderTotal = orderHeader.OrderTotal,
                PaymentIntentId = orderHeader.PaymentIntentId,
                PhoneNumber = orderHeader.PhoneNumber,
                Status = orderHeader.OrderState.ToString(),
                StripeSessionId = orderHeader.StripeSessionId,
                UserId = orderHeader.UserId,
                OrderDetails = orderDetailsList
            };

            return Result.Success(orderResponse);
        }
    }
}

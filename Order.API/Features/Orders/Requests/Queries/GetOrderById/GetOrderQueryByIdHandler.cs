using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.API.Common.Handler;
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
            var orderHeader = await _context.OrderHeaders.Include(o => o.OrderDetails).FirstAsync(o => o.Id == request.orderId);

            var orderDetailsList = new List<OrderDetailsResponseDto>();

            foreach(var item in orderHeader.OrderDetails)
            {
                var orderDetails = new OrderDetailsResponseDto
                {
                    Id = item.Id,
                    Count = item.Count,
                    OrderHeaderId = item.OrderHeaderId,
                    Price = item.Price,
                    Product = item.Product,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                };
                orderDetailsList.Add(orderDetails);
            }

            var orderResponse = new OrderHeaderResponseDto
            {
                Id = orderHeader.Id,
                CouponCode = orderHeader.CouponCode,
                Discount = orderHeader.Discount,
                Email = orderHeader.Email,
                Name = orderHeader.Name,
                OrderTime = orderHeader.OrderTime,
                OrderTotal = orderHeader.OrderTotal,
                PaymentIntentId = orderHeader.PaymentIntentId,
                PhoneNumber = orderHeader.PhoneNumber,
                Status = orderHeader.Status,
                StripeSessionId = orderHeader.StripeSessionId,
                UserId = orderHeader.UserId,
                OrderDetails = orderDetailsList
            };

            return await Result<OrderHeaderResponseDto>.SuccessAsync(orderResponse, "GetAll Orders Successfully", true);
        }
    }
}

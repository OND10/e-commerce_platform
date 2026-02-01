using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Features.Orders.Requests.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<IEnumerable<OrderHeaderResponseDto>>>
    {
        private readonly AppDbContext _context;

        public GetOrderQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<OrderHeaderResponseDto>>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OrderHeaderResponseDto> result;

            if (request.Role == "Admin")
            {
                result = await GetAllOrders(cancellationToken);
            }
            else
            {
                result = await GetAllUserOrders(request.userId, cancellationToken);
            }

            return Result.Success(result);
        }

        private async Task<IEnumerable<OrderHeaderResponseDto>> GetAllOrders(CancellationToken cancellationToken)
        {
            var orderHeaderList = await _context.OrderHeaders
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.Id)
                .ToListAsync(cancellationToken);

            return MapToDto(orderHeaderList);
        }

        private async Task<IEnumerable<OrderHeaderResponseDto>> GetAllUserOrders(string? userId, CancellationToken cancellationToken)
        {
            var orderHeaderList = await _context.OrderHeaders
                .Include(o => o.OrderDetails)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Id)
                .ToListAsync(cancellationToken);

            return MapToDto(orderHeaderList);
        }

        private IEnumerable<OrderHeaderResponseDto> MapToDto(List<OrderHeader> orderHeaders)
        {
            var dtoList = new List<OrderHeaderResponseDto>();

            foreach (var orderHeader in orderHeaders)
            {
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
                    Email = orderHeader.EmailAddress, // Correct property
                    Name = orderHeader.Name,
                    OrderTime = orderHeader.OrderTime,
                    OrderTotal = orderHeader.OrderTotal,
                    PaymentIntentId = orderHeader.PaymentIntentId,
                    PhoneNumber = orderHeader.PhoneNumber,
                    Status = orderHeader.OrderState.ToString(), // Correct property
                    StripeSessionId = orderHeader.StripeSessionId,
                    UserId = orderHeader.UserId,
                    OrderDetails = orderDetailsList
                };

                dtoList.Add(orderResponse);
            }

            return dtoList;
        }
    }
}

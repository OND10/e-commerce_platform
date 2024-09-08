using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.API.Common.Handler;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Orders.Dtos.Response;
using System.Security.Claims;

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
            var result = default(Result<IEnumerable<OrderHeaderResponseDto>>); // Specify the type for default

            string[] validProcesses = { "Admin", "User" };
            var obj = new Dictionary<string, Func<Task<Result<IEnumerable<OrderHeaderResponseDto>>>>>()
            {
                    {"Admin", async() => await GetAllOrders()},
                    {"User", async() => await GetAllUserOrders(request.userId)}
            };

            result = await obj[request.Role]();


            return await Result<IEnumerable<OrderHeaderResponseDto>>.SuccessAsync(result.Data, result.Message, true);
        }
        private async Task<Result<IEnumerable<OrderHeaderResponseDto>>>GetAllOrders()
        {

            var orderHeaderDtoList = new List<OrderHeaderResponseDto>();
            var orderDetailsList = new List<OrderDetailsResponseDto>();

            var orderHeaderList = await _context.OrderHeaders.Include(o => o.OrderDetails).OrderByDescending(o => o.Id).ToListAsync();

            foreach (var orderHeader in orderHeaderList)
            {

                foreach (var item in orderHeader.OrderDetails)
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

                orderHeaderDtoList.Add(orderResponse);
            }


            return await Result<IEnumerable<OrderHeaderResponseDto>>.SuccessAsync(orderHeaderDtoList, "GetAll Orders Successfully", true);
        }
        private async Task<Result<IEnumerable<OrderHeaderResponseDto>>> GetAllUserOrders(string? userId = "")
        {
            var orderHeaderDtoList = new List<OrderHeaderResponseDto>();
            var orderDetailsList = new List<OrderDetailsResponseDto>();

            var orderHeaderList = await _context.OrderHeaders.Include(o => o.OrderDetails).Where(o=> o.UserId == userId).OrderByDescending(o => o.Id).ToListAsync();

            orderHeaderDtoList = new List<OrderHeaderResponseDto>();

            orderDetailsList = new List<OrderDetailsResponseDto>();


            foreach (var orderHeader in orderHeaderList)
            {

                foreach (var item in orderHeader.OrderDetails)
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

                orderHeaderDtoList.Add(orderResponse);
            }

            return await Result<IEnumerable<OrderHeaderResponseDto>>.SuccessAsync(orderHeaderDtoList, "GetAll User Orders Successfully", true);

        }
    }
}

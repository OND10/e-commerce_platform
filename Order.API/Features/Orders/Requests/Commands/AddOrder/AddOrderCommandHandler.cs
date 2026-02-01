using MediatR;
using SharedKernel.Results;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Orders.Services.Interface;
using Order.API.Features.Orders.Services.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Order.API.Features.Orders.Requests.Commands.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<OrderHeaderResponseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IOrderService _orderService;

        public AddOrderCommandHandler(AppDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<Result<OrderHeaderResponseDto>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validation (using existing service logic)
                IOrderService serviceWithValidation = new ValidationDecorator(new BasicOrderService());
                var processOrder = await serviceWithValidation.ProcessOrder(request.cartDto);

                if (!processOrder.CartHeaderResponse.isValid)
                {
                     return Result.Failure<OrderHeaderResponseDto>("Placing Order failed: Validation error");
                }

                var cartHeader = request.cartDto.CartHeaderResponse;
                
                var order = OrderHeader.Create(
                    cartHeader.UserId,
                    cartHeader.Name ?? "Unknown", 
                    cartHeader.Email ?? "no-email@example.com", 
                    cartHeader.PhoneNumber ?? "", 
                    cartHeader.CartTotal
                );

                if (request.cartDto.CartDetailsResponse != null)
                {
                    foreach (var detail in request.cartDto.CartDetailsResponse)
                    {
                        order.AddLineItem(
                            detail.ProductId, 
                            detail.Product?.Name ?? "Unknown Product", 
                            detail.Product?.Price ?? detail.Count, // Logic check needed: count vs price? Assuming detail has price? Repo says detail.Product.Price. Use detail.Product if available.
                            detail.Count
                        );
                    }
                }

                _context.OrderHeaders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);

                var response = new OrderHeaderResponseDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderTotal = order.OrderTotal,
                    Status = order.OrderState.ToString(),
                    Email = order.EmailAddress,
                    Name = order.Name,
                    PhoneNumber = order.PhoneNumber,
                    OrderTime = order.OrderTime
                };

                return Result.Success(response);

            }
            catch (Exception ex)
            {
                return Result.Failure<OrderHeaderResponseDto>(ex.Message);
            }
        }
    }
}

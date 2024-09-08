using MediatR;
using Order.API.Common.Handler;
using Order.API.DataBase;
using Order.API.Entities;
using Order.API.Features.Orders.Dtos.Response;
using Order.API.Features.Orders.Services.Implementation;
using Order.API.Features.Orders.Services.Interface;

namespace Order.API.Features.Orders.Requests.Commands.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<OrderHeaderResponseDto>>
    {
        private readonly AppDbContext _context;
        protected readonly IOrderService _orderService;
        public AddOrderCommandHandler(AppDbContext context , IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<Result<OrderHeaderResponseDto>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderHeaderResponse = new OrderHeaderResponseDto();

                var mappedorderHeaderResponse = orderHeaderResponse.ToResponse(request);

                // Add each item from CartDetailsResponse to OrderDetailsResponseDto
                foreach (var cartDetail in request.cartDto.CartDetailsResponse)
                {
                    var orderDetail = new OrderDetailsResponseDto
                    {
                        OrderHeaderId = mappedorderHeaderResponse.Id,
                        ProductId = cartDetail.ProductId,
                        Count = cartDetail.Count,
                        Product = cartDetail.Product,
                        ProductName = cartDetail.Product.Name,
                        Price = cartDetail.Product.Price
                    };
                    mappedorderHeaderResponse.OrderDetails.Add(orderDetail); // Use Add
                }

                // Map OrderHeaderResponseDto to OrderHeader
                var orderHeader = new OrderHeader();

                var mappedorderHeader = orderHeader.ToModel(mappedorderHeaderResponse);

                // Add each item from OrderDetailsResponseDto to OrderDetails
                foreach (var orderDetailResponse in mappedorderHeaderResponse.OrderDetails)
                {
                    var orderDetail = new OrderDetails
                    {
                        OrderHeaderId = mappedorderHeaderResponse.Id,
                        ProductId = orderDetailResponse.ProductId,
                        Count = orderDetailResponse.Count,
                        Product = orderDetailResponse.Product,
                        ProductName = orderDetailResponse.ProductName,
                        Price = orderDetailResponse.Price
                    };
                    mappedorderHeader.OrderDetails.Add(orderDetail); // Use Add
                }

                // Order Processing to make sure that order is placed
                IOrderService _orderService = new BasicOrderService();

                _orderService = new ValidationDecorator(_orderService);

                var processOrder = await _orderService.ProcessOrder(request.cartDto);

                if(processOrder.CartHeaderResponse.isValid == true)
                {
                    var create = _context.OrderHeaders.Add(mappedorderHeader).Entity;
                    await _context.SaveChangesAsync();

                    mappedorderHeaderResponse.Id = create.Id;

                    return await Result<OrderHeaderResponseDto>.SuccessAsync(mappedorderHeaderResponse, "Added Successfully", true);
                }

                else
                {
                    return await Result<OrderHeaderResponseDto>.FaildAsync(false, $"Placing Order is faild");
                }

            }
            catch (Exception ex)
            {
                return await Result<OrderHeaderResponseDto>.FaildAsync(false, $"{ex.Message}");
            }
        }

    }
}

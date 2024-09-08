using Order.API.Common.Enum;
using Order.API.Features.Orders.Requests.Commands.AddOrder;

namespace Order.API.Features.Orders.Dtos.Response
{
    public class OrderHeaderResponseDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Status { get; set; }
        //This properties for payment mechanism
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        //One to may relationship between the orderheader and orderdetails
        public List<OrderDetailsResponseDto> OrderDetails { get; set; }


        public OrderHeaderResponseDto ToResponse(AddOrderCommand command)
        {
            var response = new OrderHeaderResponseDto
            {
                UserId = command.cartDto.CartHeaderResponse.UserId,
                CouponCode = command.cartDto.CartHeaderResponse.CouponCode,
                Discount = command.cartDto.CartHeaderResponse.Discount,
                OrderTotal = command.cartDto.CartHeaderResponse.CartTotal,
                Name = command.cartDto.CartHeaderResponse.Name,
                Email = command.cartDto.CartHeaderResponse.Email,
                PhoneNumber = command.cartDto.CartHeaderResponse.PhoneNumber,
                OrderTime = DateTime.Now,
                Status = StatusEnum.Status_Pending,
                OrderDetails = new List<OrderDetailsResponseDto>()
            };

            return response;
        }
    }
}

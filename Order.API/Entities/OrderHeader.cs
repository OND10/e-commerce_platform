using Order.API.Features.Orders.Dtos.Response;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Entities
{
    public class OrderHeader
    {
        [Key] 
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime OrderTime {  get; set; }
        public string? Status { get; set; }
        //This properties for payment mechanism
        public string? PaymentIntentId {  get; set; }
        public string? StripeSessionId {  get; set; }
        //One to may relationship between the orderheader and orderdetails
        public List<OrderDetails>? OrderDetails { get; set; }

        public OrderHeader ToModel(OrderHeaderResponseDto response)
        {
            return new OrderHeader
            {
                UserId = response.UserId,
                CouponCode = response.CouponCode,
                Discount = response.Discount,
                OrderTotal = response.OrderTotal,
                Name = response.Name,
                Email = response.Email,
                PhoneNumber = response.PhoneNumber,
                Status = response.Status,
                OrderTime = response.OrderTime,
                OrderDetails = new List<OrderDetails>() // Change to List
            };
        }
    }
}

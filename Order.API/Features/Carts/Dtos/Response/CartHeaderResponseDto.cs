namespace Order.API.Features.Carts.Dtos.Response
{
    public class CartHeaderResponseDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; } 
        public string? Name {  get; set; }
        public string? Email { get; set; }
        public bool isValid { get; set; } = false;
        public string? PhoneNumber {  get; set; }
    }
}

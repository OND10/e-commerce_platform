namespace Order.API.Features.Carts.Dtos.Request
{
    public class CartDetailsRequestDto
    {
        //public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}

using Order.API.Features.Products.Dtos.Response;

namespace Order.API.Features.Orders.Dtos.Response
{
    public class OrderDetailsResponseDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductResponseDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
    }
}

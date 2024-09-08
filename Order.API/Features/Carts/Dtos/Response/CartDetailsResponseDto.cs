using Order.API.Features.Products.Dtos.Response;
namespace Order.API.Features.Carts.Dtos.Response
{
    public class CartDetailsResponseDto
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderResponseDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductResponseDto? Product { get; set; }
        public int Count { get; set; }
    }
}

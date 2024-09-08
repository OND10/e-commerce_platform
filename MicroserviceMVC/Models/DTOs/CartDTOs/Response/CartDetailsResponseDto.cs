using eCommerceWebMVC.Models.DTOs.ProductDTOs.Response;

namespace eCommerceWebMVC.Models.DTOs.CartDTOs.Response
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

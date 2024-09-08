using ShoppingCart.API.Entities;
using ShoppingCart.API.Features.DTOs.ProductDTOs;

namespace ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response
{
    public class CartDetailsResponseDto
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductResponseDto? Product { get; set; }
        public int Count { get; set; }
    }
}

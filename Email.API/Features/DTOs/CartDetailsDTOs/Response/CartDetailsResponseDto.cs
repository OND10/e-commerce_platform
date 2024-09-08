using Email.API.Features.DTOs.CartHeaderDTOs.Response;
using Email.API.Features.DTOs.ProductDTOs;

namespace Email.API.Features.DTOs.CartDetailsDTOs.Response
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

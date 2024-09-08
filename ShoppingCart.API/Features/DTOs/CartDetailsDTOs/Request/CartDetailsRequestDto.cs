using ShoppingCart.API.Entities;
using ShoppingCart.API.Features.DTOs.ProductDTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Request
{
    public class CartDetailsRequestDto
    {
        //public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}

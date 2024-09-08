using ShoppingCart.API.Features.DTOs.ProductDTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ShoppingCart.API.Entities
{
    [DebuggerDisplay("{Name}")]
    public class CartDetails
    {
        [Key]
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey(nameof(CartHeaderId))]
        public CartHeader CartHeader { get; set;}
        public int ProductId {  get; set; }
        [NotMapped]
        public ProductResponseDto Product { get; set; }
        public int Count {  get; set; }
    }
}

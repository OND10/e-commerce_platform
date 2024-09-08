using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Order.API.Features.Products.Dtos.Response;
namespace Order.API.Entities
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey(nameof(OrderHeaderId))]
        public OrderHeader? OrderHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductResponseDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName {  get; set; }
        public double? Price { get; set; }
    }
}

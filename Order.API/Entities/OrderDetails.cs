using Common.BuildingBlocks.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using Order.API.Features.Products.Dtos.Response;

namespace Order.API.Entities
{
    // Renamed to Item for clarity, or kept as OrderDetails but as child entity
    public class OrderDetails : Entity
    {
        public int OrderHeaderId { get; private set; }
        // Navigation - removing standard public setter. 
        // EF Core can set this via backing field or constructor if configured, 
        // but typically child entities don't strictly need navigation back to parent 
        // if treated as part of aggregate. Keeping for EF convenience.
        public OrderHeader? OrderHeader { get; private set; }
        
        public int ProductId { get; private set; }
        
        [NotMapped]
        public ProductResponseDto? Product { get; private set; } // Kept from original, might be DTO projection
        
        public int Count { get; private set; }
        public string ProductName { get; private set; }
        public double? Price { get; private set; }

        private OrderDetails() { }

        public OrderDetails(int productId, string productName, double price, int count)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Count = count;
        }

        public void SetProductDto(ProductResponseDto dto)
        {
            Product = dto;
        }
    }
}

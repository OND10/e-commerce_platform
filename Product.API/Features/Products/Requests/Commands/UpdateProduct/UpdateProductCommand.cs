using SharedKernel.Abstractions.Messaging;
using SharedKernel.Results;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Commands.UpdateProduct
{
    public class UpdateProductCommand : ICommand<ProductResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberofProduct { get; set; }
        public string Category { get; set; } = string.Empty;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}

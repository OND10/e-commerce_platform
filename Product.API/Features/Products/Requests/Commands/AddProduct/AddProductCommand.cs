using SharedKernels.Abstractions.Messaging;
using SharedKernels.Results;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Commands.AddProduct
{
    public class AddProductCommand : ICommand<ProductResponseDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberofProduct { get; set; }
        public string Category { get; set; } = string.Empty;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}

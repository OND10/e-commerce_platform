using MediatR;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;

namespace Product.API.Features.Products.Requests.Commands.AddProduct
{
    public class AddProductCommand : IRequest<Result<ProductResponseDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberofProduct { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}

using MediatR;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Features.Products.Requests.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result<ProductResponseDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberofProduct { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}

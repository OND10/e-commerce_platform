using AutoMapper;
using Product.API.Entities;
using Product.API.Features.Products.DTOs;

namespace Product.API.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile() 
        {
            CreateMap<ProductRequestDto, Product.API.Entities.Product>();
            CreateMap<Product.API.Entities.Product, ProductRequestDto>();
            CreateMap<Product.API.Entities.Product, ProductResponseDto>();
        }

    }
}

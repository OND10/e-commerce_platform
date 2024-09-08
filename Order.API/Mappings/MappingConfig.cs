using AutoMapper;
using Order.API.Entities;
using Order.API.Features.Carts.Dtos.Response;
using Order.API.Features.Orders.Dtos.Request;
using Order.API.Features.Orders.Dtos.Response;

namespace Order.API.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration(config =>
            {
                //Mapping the CartHeader to the OrderHeader using config autoMapper
                config.CreateMap<OrderHeaderResponseDto, CartHeaderResponseDto>()
                .ForMember(dest=> dest.CartTotal, u=> u.MapFrom(src=> src.OrderTotal)).ReverseMap();


                config.CreateMap<CartDetailsResponseDto, OrderDetailsResponseDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price)).ReverseMap();

                config.CreateMap<OrderHeader, OrderHeaderResponseDto>().ReverseMap();
                config.CreateMap<OrderDetailsResponseDto, OrderDetails>().ReverseMap();
            });

            return mappingconfig;
        }
    }
}

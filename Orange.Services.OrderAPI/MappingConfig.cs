using System.Net.WebSockets;
using AutoMapper;
using Orange.Services.OrderAPI.Models;
using Orange.Services.OrderAPI.Models.Dto;


namespace Orange.Services.OrderAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(
                    dest => dest.CartTotal, 
                    opt => 
                        opt.MapFrom(src => src.OrderTotal
                        ))
                .ReverseMap();
            config.CreateMap<CartDetailsDto, OrderDetailDto>()
                .ForMember(dest =>dest.ProductName, src => src.MapFrom(sr => sr.Product.Name))
                .ForMember(dest =>dest.Price, src => src.MapFrom(sr => sr.Product.Price));
            
            config.CreateMap<OrderDetailDto, CartDetailsDto>();
            
            config.CreateMap<OrderHeader,OrderHeaderDto>().ReverseMap();
            config.CreateMap<OrderDetails, OrderDetailDto>().ReverseMap();

        });
        
        return mappingConfig;
    }
}
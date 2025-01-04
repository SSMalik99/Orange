using AutoMapper;
using Orange.Services.ShoppingCartAPI.Models;
using Orange.Services.ShoppingCartAPI.Models.Dto;


namespace Orange.Services.ShoppingCartAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader,CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        });
        
        return mappingConfig;
    }
}
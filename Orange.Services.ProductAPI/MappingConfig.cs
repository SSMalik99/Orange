using AutoMapper;
using Orange.Services.ProductAPI.Models;
using Orange.Services.ProductAPI.Models.Dto;


namespace Orange.Services.ProductAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Product, ProductDto>().ReverseMap();
            config.CreateMap<ProductDto, Product>().ReverseMap();
            config.CreateMap<CategoryDto, Category>().ReverseMap();
            config.CreateMap<Category, CategoryDto>().ReverseMap();
        });
        
        return mappingConfig;
    }
}
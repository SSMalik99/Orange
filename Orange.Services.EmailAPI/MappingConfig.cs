using AutoMapper;



namespace Orange.Services.EmailAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            //config.CreateMap<Product, ProductDto>().ReverseMap();
            
        });
        
        return mappingConfig;
    }
}
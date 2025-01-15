using AutoMapper;



namespace Orange.Services.RewardAPI;

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
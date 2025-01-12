using AutoMapper;



namespace Orange.Services.OrderAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            //config.CreateMap<CartHeader,CartHeaderDto>().ReverseMap();
            
        });
        
        return mappingConfig;
    }
}
using AutoMapper;



namespace Orange.Services.AuthAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            // config.CreateMap<Coupon, CouponDto>().ReverseMap();
            // config.CreateMap<CouponDto, Coupon>().ReverseMap();
        });
        
        return mappingConfig;
    }
}
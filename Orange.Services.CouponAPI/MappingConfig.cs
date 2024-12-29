using AutoMapper;
using Orange.Services.CouponAPI.Dto;
using Orange.Services.CouponAPI.Models;

namespace Orange.Services.CouponAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Coupon, CouponDto>().ReverseMap();
            config.CreateMap<CouponDto, Coupon>().ReverseMap();
        });
        
        return mappingConfig;
    }
}
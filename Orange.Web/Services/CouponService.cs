using Orange.Web.Models;
using Orange.Web.Models.Coupon;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Services;

public class CouponService(IBaseService baseService) : ICouponService
{
    
    public async Task<ResponseDto> GetCouponAsync(string couponCode)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.CouponApiBase+"/api/coupon/GetByCode/"+couponCode,
        });
    }

    public async Task<ResponseDto> GetAllCouponAsync()
    {
        return await baseService.SendAsync(new RequestDto()
        {
            Url = SharedDetail.CouponApiBase+"/api/coupon",
        });
    }

    public async Task<ResponseDto> GetCouponById(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.CouponApiBase+"/api/coupon/"+id,
        });
    }

    public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.CouponApiBase+"/api/coupon/",
            Body = couponDto,
        });
    }

    public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Put,
            Url = SharedDetail.CouponApiBase+"/api/coupon/",
            Body = couponDto,
        });
    }

    public async Task<ResponseDto> DeleteCouponAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Delete,
            Url = SharedDetail.CouponApiBase+"/api/coupon/"+id,
        });
    }
}
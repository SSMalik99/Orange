using Orange.Services.ShoppingCartAPI.Models.Dto;

namespace Orange.Services.OrderAPI.Services.IServices;

public interface ICouponService
{
    Task<CouponDto?> GetCouponByCode(string couponCode);
}
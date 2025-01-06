using Orange.Web.Models;
using Orange.Web.Models.Cart;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Services;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;

    public CartService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    
    
    public async Task<ResponseDto> GetCartAsync(string userId)
    {
        return await _baseService.SendAsync(new RequestDto() {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.CartApiBase+"/api/cart/GetCart/"+userId
        });
    }

    public async Task<ResponseDto> UpsertCartAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto() {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.CartApiBase+"/api/cart/CartUpsert/",
            Body = cartDto
        });
    }

    public async Task<ResponseDto> RemoveFromCartAsync(Guid cartDetailId)
    {
        return await _baseService.SendAsync(new RequestDto() {
            ApiType = SharedDetail.ApiType.Delete,
            Url = SharedDetail.CartApiBase+"/api/cart/CartDelete/"+cartDetailId
        });
    }

    public async Task<ResponseDto> ApplyCouponAsync(ApplyCouponDto applyCouponDto)
    {
        return await _baseService.SendAsync(new RequestDto() {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.CartApiBase+"/api/cart/ApplyCoupon/",
            Body = applyCouponDto
        });
    }
}
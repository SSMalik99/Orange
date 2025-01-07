using Orange.Web.Models;
using Orange.Web.Models.Cart;

namespace Orange.Web.Services.IService;

public interface ICartService
{
    Task<ResponseDto> GetCartAsync(string userId);
    Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
    Task<ResponseDto> RemoveFromCartAsync(string cartDetailId);
    Task<ResponseDto> ApplyCouponAsync(ApplyCouponDto applyCouponDto);
}
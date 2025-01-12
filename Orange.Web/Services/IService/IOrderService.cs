using Orange.Web.Models;
using Orange.Web.Models.Cart;
using Orange.Web.Models.Coupon;

namespace Orange.Web.Services.IService;

public interface IOrderService
{
    Task<ResponseDto> CreateOrderAsync(CartDto cartDto);
    
    
}
using Orange.Web.Models;
using Orange.Web.Models.Cart;
using Orange.Web.Models.Coupon;
using Orange.Web.Models.Order;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Services;

public class OrderService(IBaseService baseService) : IOrderService
{

    public async Task<ResponseDto> CreateOrderAsync(CartDto cartDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.OrderApiBase+"/api/order/create",
            Body = cartDto,
        });
        
        
    }

    public async Task<ResponseDto> CreatePaymentSessionAsync(StripeRequestDto stripeRequestDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.OrderApiBase+"/api/order/CreatePaymentSession",
            Body = stripeRequestDto,
        });
    }
}
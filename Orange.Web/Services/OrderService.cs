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

    public async Task<ResponseDto> ValidateStripeSessionAsync(string orderHeaderId)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.OrderApiBase+"/api/order/ValidateStripeSession",
            Body = orderHeaderId,
        });
    }

    public async Task<ResponseDto> GetUserOrdersAsync(string? userId, int? limit = 10, int? page = 1)
    {
        var url = SharedDetail.OrderApiBase+"/api/order/GetUserOrders?";
        
        if (userId is not null)
        {
            url += $"userId={userId}&";
            
        }
        
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = url+"limit="+limit+"&page="+page,
        });
    }

    public async Task<ResponseDto> GetOrderAsync(string orderHeaderId)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.OrderApiBase+"/api/order/"+orderHeaderId
        });
    }

    public async Task<ResponseDto> UpdateOrderStatusAsync(Guid orderId, string status)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.OrderApiBase+"/api/order/"+orderId+"/UpdateOrderStatus/",
            Body = status,
        });
    }
}
using Orange.Web.Models;
using Orange.Web.Models.Cart;
using Orange.Web.Models.Order;

namespace Orange.Web.Services.IService;

public interface IOrderService
{
    Task<ResponseDto> CreateOrderAsync(CartDto cartDto);
    Task<ResponseDto> CreatePaymentSessionAsync(StripeRequestDto stripeRequestDto);
    
    Task<ResponseDto> ValidateStripeSessionAsync(string orderHeaderId);
    Task<ResponseDto> GetUserOrdersAsync(string? userId, int? limit = 10, int? page = 1);
    Task<ResponseDto> GetOrderAsync(string orderHeaderId);
    Task<ResponseDto> UpdateOrderStatusAsync(string orderId, string status);
    
    
}
using Orange.Web.Models;
using Orange.Web.Models.Cart;
using Orange.Web.Models.Order;

namespace Orange.Web.Services.IService;

public interface IOrderService
{
    Task<ResponseDto> CreateOrderAsync(CartDto cartDto);
    Task<ResponseDto> CreatePaymentSessionAsync(StripeRequestDto stripeRequestDto);
    
    Task<ResponseDto> ValidateStripeSessionAsync(string orderHeaderId);
    
    
}
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models.Cart;
using Orange.Web.Models.Order;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    public CartController(ICartService cartService, IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await LoadCartInformation());
    }
    
    
    [Authorize]
    [HttpGet("cart/checkout")]
    public async Task<IActionResult> Checkout()
    {
        return View(await LoadCartInformation());
    }

    [HttpPost("RemoveItem/{cartDetailsId}")]
    public async Task<IActionResult> RemoveItem(string cartDetailsId)
    {
        var responseDto = await _cartService.RemoveFromCartAsync( cartDetailsId );

        if(responseDto.IsSuccess) { TempData[NotificationType.Success] = responseDto.Message;
        }else{ TempData[NotificationType.Error] = responseDto.Message; }
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
    {
        if (string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
        {
            TempData[NotificationType.Error] = "Please enter a valid coupon code.";
            return RedirectToAction(nameof(Index));
        }
        
        ApplyCouponDto applyCouponDto = new ApplyCouponDto()
        {
            UserId = cartDto.CartHeader.UserId,
            CouponCode = cartDto.CartHeader.CouponCode,
        };
        var responseDto = await _cartService.ApplyCouponAsync(applyCouponDto);
        if (responseDto.IsSuccess)
        {
            TempData[NotificationType.Success] = responseDto.Message;
        }
        else
        {
            TempData[NotificationType.Error] = responseDto.Message;
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
    {
        ApplyCouponDto applyCouponDto = new ApplyCouponDto()
        {
            UserId = cartDto.CartHeader.UserId,
            CouponCode = null,
        };
        
        var responseDto = await _cartService.ApplyCouponAsync(applyCouponDto);
        if (responseDto.IsSuccess)
        {
            TempData[NotificationType.Success] = "Coupon code removed successfully.";
            
        }
        else
        {
            TempData[NotificationType.Error] = responseDto.Message;
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> EmailCart()
    {
        var cartDto = await LoadCartInformation();
        cartDto.CartHeader.Email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        var responseDto = await _cartService.EmailCartAsync(cartDto);
        if (responseDto.IsSuccess)
        {
            TempData[NotificationType.Success] = responseDto.Message;
        }
        else
        {
            TempData[NotificationType.Error] = responseDto.Message;
        }
        return RedirectToAction(nameof(Index));
        
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartDto cartDto)
    {
        var cart = await LoadCartInformation();
        cart.CartHeader.Email = cartDto.CartHeader.Email;
        cart.CartHeader.FirstName = cartDto.CartHeader.FirstName;
        cart.CartHeader.LastName = cartDto.CartHeader.LastName;
        cart.CartHeader.PhoneNumber = cartDto.CartHeader.PhoneNumber;
        
        var response = await _orderService.CreateOrderAsync(cart);
        if (response.IsSuccess)
        {
            var orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Data));
            
            
            var domain = Request.Scheme + "://" + Request.Host.Value;
            var stripeDto = new StripeRequestDto
            {
                ApprovedUrl = domain + "/cart/Confirmation?orderId="+orderHeaderDto.Id,
                CancelUrl = domain + "/cart/checkout",
                OrderHeader = orderHeaderDto
            };
            var stripeResponse = await _orderService.CreatePaymentSessionAsync(stripeDto);
            
            if (stripeResponse.IsSuccess)
            {
                var stripeResponseData = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeResponse.Data));
                Response.Headers.Append("Location", stripeResponseData?.StripeSessionUrl);

                return new StatusCodeResult(StatusCodes.Status303SeeOther);
            }
            
            TempData[NotificationType.Error] = stripeResponse.Message;
            return RedirectToAction(nameof(Checkout));
        }
        
        TempData[NotificationType.Error] = response.Message;
        return View(cartDto);
    }

    
    public  IActionResult Confirmation(string orderId)
    {
        return View(orderId);
        
    }
    
    
    private async Task<CartDto> LoadCartInformation()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        var responseDto = await _cartService.GetCartAsync( userId );
        if (!responseDto.IsSuccess) return new CartDto();
        
        var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Data));
        return cartDto;

    }
    
    
}
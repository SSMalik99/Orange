using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models.Cart;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await LoadCartInformation());
    }
    
    
    [Authorize]
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
        Console.WriteLine(cartDto.CartHeader.Email);
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
    private async Task<CartDto> LoadCartInformation()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        var responseDto = await _cartService.GetCartAsync( userId );
        if (!responseDto.IsSuccess) return new CartDto();
        
        var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Data));
        return cartDto;

    }
}
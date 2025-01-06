using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models.Cart;
using Orange.Web.Services.IService;

namespace Orange.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await LoadCartInformation());
    }

    private async Task<CartDto> LoadCartInformation()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
        Console.WriteLine(userId);
        
        var responseDto = await _cartService.GetCartAsync( userId );
        if (!responseDto.IsSuccess) return new CartDto();
        
        var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Data));
        return cartDto;

    }
}
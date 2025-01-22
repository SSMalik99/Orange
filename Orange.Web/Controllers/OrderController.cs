using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Models.Order;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    public async  Task<IActionResult> Detail(string orderId)
    {
        OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
        var userId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        
        var response = await _orderService.GetOrderAsync(orderId);
        if (response.IsSuccess)
        {
            orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Data));
        }

        if (!User.IsInRole(SharedDetail.RoleAdmin) && userId != null && orderHeaderDto?.UserId != userId)
        {
            return Unauthorized();
        }
        
        return View(orderHeaderDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders(int? limit = 10, int? page = 1)
    {
        IEnumerable<OrderHeaderDto> orders = [];
        // PaginateDto ordersPaginateDto = new PaginateDto()
        // {
        //     Limit = limit??10,
        //     CurrentPage = page ?? 1,
        //     Main = orders
        // };
        

        var userId = string.Empty;

        
        if (!User.IsInRole(SharedDetail.RoleAdmin))
        {
        
            userId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        }
        
        var responseDto = await _orderService.GetUserOrdersAsync(userId, limit, page);
        
        if (responseDto.IsSuccess)
        {
            //ordersPaginateDto = JsonConvert.DeserializeObject<PaginateDto>(Convert.ToString(responseDto.Data));
            orders = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(responseDto.Data));
        }
        
        return Json(new { data = orders }); 

    }
    
}
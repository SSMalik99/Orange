using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
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
        Console.WriteLine(JsonConvert.SerializeObject(response));
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
    public async Task<IActionResult> GetAllOrders(int? limit = 10, int? page = 1, string status = "All")
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
            switch (status)
            {
                case "approved":
                    orders = orders?.Where(u => u.Status == OrderStatus.Approved);
                    break;
                case "readyforpickup":
                    orders = orders?.Where(u => u.Status == OrderStatus.ReadyForPickup);
                    break;
                case "cancelled":
                    orders = orders?.Where(u => u.Status == OrderStatus.Cancelled || u.Status ==OrderStatus.Refunded);
                    break;
                default:
                    break;
            }
        }
        
        return Json(new { data = orders }); 

    }
    
    [HttpPost]
    public async Task<IActionResult> CompleteOrder(string orderHeaderId)
    {
        
        var response = await _orderService.UpdateOrderStatusAsync(orderHeaderId, OrderStatus.Completed);
        if (response.IsSuccess)
        {
            TempData[NotificationType.Success] = "Order is marked completed now.";
        }
        else
        {
            TempData[NotificationType.Error] = response.Message;
        }
        
        return RedirectToAction(nameof(Detail), new { orderId = orderHeaderId });
    }
    
    
    [HttpPost]
    public async Task<IActionResult> OrderReadyForPickup(string orderHeaderId)
    {
        
        var response = await _orderService.UpdateOrderStatusAsync(orderHeaderId, OrderStatus.ReadyForPickup);
        if (response.IsSuccess)
        {
            TempData[NotificationType.Success] = "Order is now ready to pickup";
        }
        else
        {
            TempData[NotificationType.Error] = response.Message;
        }
        
        return RedirectToAction(nameof(Detail), new { orderId = orderHeaderId });
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CancelOrder(string orderHeaderId)
    {
        
        var response = await _orderService.UpdateOrderStatusAsync(orderHeaderId, OrderStatus.Cancelled);
        if (response.IsSuccess)
        {
            TempData[NotificationType.Success] ="Order is marked as cancelled";
        }
        else
        {
            TempData[NotificationType.Error] = response.Message;
        }
        
        return RedirectToAction(nameof(Detail), new { orderId = orderHeaderId });
    }

    
    
}
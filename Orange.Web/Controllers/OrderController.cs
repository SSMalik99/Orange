using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Orange.Web.Services.IService;

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

    [HttpGet]
    public IActionResult GetAllOrders()
    {
        throw new ExternalException();

    }
    
}
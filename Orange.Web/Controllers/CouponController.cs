using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Models.Coupon;
using Orange.Web.Services;
using Orange.Web.Services.IService;

namespace Orange.Web.Controllers;

public class CouponController : Controller
{
    private readonly ICouponService _couponService;
    public CouponController(ICouponService couponService)
    {
        this._couponService = couponService;
    }
    public async  Task<IActionResult> Index()
    {
        List<CouponDto>? coupons = new();
        var response = await _couponService.GetAllCouponAsync();
        if (response.IsSuccess)
        {
            coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Data));
        }
        return View(coupons);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CouponDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _couponService.CreateCouponAsync(model);
            if (response.IsSuccess)
            {
                
            }
        }
        return View(model);
        
    }
}
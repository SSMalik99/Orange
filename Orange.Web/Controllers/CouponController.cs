using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Models.Coupon;
using Orange.Web.Services;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class CouponController : Controller
{
    private readonly ICouponService _couponService;
    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }
    
    /*
     * Index Method to get all the coupons
     */
    public async  Task<IActionResult> Index()
    {
        List<CouponDto>? coupons = new();
        var response = await _couponService.GetAllCouponAsync();
        if (response.IsSuccess)
        {
            coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Data));
        }
        else
        {
            TempData[NotificationType.Error] = response.Message;
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
        if (!ModelState.IsValid) return View(model);
        
        var response = await _couponService.CreateCouponAsync(model);
        
        if(response.IsSuccess) 
        {
            TempData[NotificationType.Success] = response.Message;
            return RedirectToAction("Index");
        }
        
        TempData[NotificationType.Error] = response.Message;
        return View(model);
        
    }
    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var response = await _couponService.GetCouponById(id.Value);
        
        if (!response.IsSuccess) return NotFound();
        var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Data));
        return View(coupon);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(CouponDto model)
    {
        var response = await _couponService.DeleteCouponAsync(model.Id);
        
        if (response.IsSuccess)
        {
            TempData[NotificationType.Success] = response.Message;
            return RedirectToAction("Index");
        }
        
        TempData[NotificationType.Error] = response.Message;
        return View(model);
        
    }
}
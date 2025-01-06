using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Models.Product;
using Orange.Web.Services;
using Orange.Web.Services.IService;

namespace Orange.Web.Controllers;

public class HomeController : Controller
{
    
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var responseDto = await _productService.GetPaginatedProductAsync();
        var paginateData = JsonConvert.DeserializeObject<PaginateDto>(Convert.ToString(responseDto.Data));
        List<ProductDto> products = [];
        if (responseDto.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(paginateData.Main));    
        }
        return View(products);
    }
    
    
    
    [HttpGet("ProductDetail/{productId:int}")]
    public async Task<IActionResult> ProductDetail(int productId)
    {
        var responseDto = await _productService.GetProductByIdAsync(productId);
        var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Data));
        
        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
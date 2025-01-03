using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orange.Web.Models;
using Orange.Web.Models.Product;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        this._productService = productService;
    }

    private async Task<List<CategoryDto>> GetCategories()
    {
        var categories = new List<CategoryDto>();
        var paginateResponse = await _productService.GetPaginatedCategoryAsync();
        if (!paginateResponse.IsSuccess) return categories;
        
        var paginated = JsonConvert.DeserializeObject<PaginateDto>(Convert.ToString(paginateResponse.Data));
        categories = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(paginated.Main));
        return categories;
    }
    
    public async Task<IActionResult> Index()
    {
        var responseDto = await _productService.GetPaginatedProductAsync();
        var paginateData = JsonConvert.DeserializeObject<PaginateDto>(Convert.ToString(responseDto.Data));
        var products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(paginateData.Main));
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await GetCategories();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        if (!ModelState.IsValid) return View(productDto);
        
        var response = await _productService.CreateProductAsync(productDto);
            
        if (response.IsSuccess)
        {
            TempData[NotificationType.Success] = response.Message;
            return RedirectToAction("Index");
        }

        TempData[NotificationType.Error] = response.Message;
        return View(productDto);


    }

    public async Task<IActionResult> Edit(int id)
    {
        
        var response = await _productService.GetProductByIdAsync(id);

        if (response.IsSuccess == false)
        {
            TempData[NotificationType.Error] = response.Message;
            return RedirectToAction(nameof(Index));
        }
        var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Data));
        
        ViewBag.Categories = await GetCategories();
        
        return View(productDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.UpdateProductAsync(productDto);
            if (response.IsSuccess)
            {
                TempData[NotificationType.Success] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            
            TempData[NotificationType.Error] = response.Message;
        }
        
        ViewBag.Categories = await GetCategories();
        return View(productDto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _productService.GetProductByIdAsync(id);
        if (response.IsSuccess == false)
        {
            TempData[NotificationType.Error] = response.Message;
            return RedirectToAction(nameof(Index));
        }
        
        var productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Data));
        Console.WriteLine(JsonConvert.SerializeObject(productDto));
        return View(productDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductDto productDto)
    {
        var response = await _productService.DeleteProductAsync(productDto.Id);
        if (response.IsSuccess)
        {
            TempData[NotificationType.Success] = response.Message;
            return RedirectToAction(nameof(Index));
        }
        TempData[NotificationType.Error] = response.Message;
        return RedirectToAction("Delete", "Product", new { id = productDto.Id });
    }
    
}
using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orange.Services.ProductAPI.Data;
using Orange.Services.ProductAPI.Models;
using Orange.Services.ProductAPI.Models.Dto;

namespace Orange.Services.ProductAPI.Controllers;

[Route("api/products")]
[Authorize]
[ApiController]
public class ProductApiController:ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ResponseDto _responseDto;
    private readonly IMapper _mapper;
    private readonly PaginateDto _paginateDto;

    public ProductApiController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _responseDto = new ResponseDto();
        _paginateDto = new PaginateDto();
        
    }

    [HttpPost("GetProductsWithIds")]
    public IActionResult GetProductsWithIds([FromBody] int[] ids)
    {
        try
        {
            var products = _dbContext.Products.Where(p => ids.Contains(p.Id)).ToList();
            _responseDto.Data = _mapper.Map<List<ProductDto>>(products);
            _responseDto.Message = "Products retrieved successfully";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
        
    }
    
    [HttpGet]
    public IActionResult Get(int limit = 20, int page = 1)
    {
        try
        {
            var products = _dbContext
                .Products
                .Include(p => p.Category)
                .Skip(limit* (page - 1))
                .Take(limit)
                .ToList();
            
            _paginateDto.CurrentPage = page;
            _paginateDto.Limit = limit;
            _paginateDto.Main = _mapper.Map<List<ProductDto>>(products);
            
            _responseDto.Data = _paginateDto;
            _responseDto.Message = "Products retrieved successfully.";
            
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        try
        {
            var product = _dbContext.Products.Include(c => c.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Product not found.";
                return NotFound(_responseDto);
            }
    
            _responseDto.Data = _mapper.Map<ProductDto>(product);
            _responseDto.Message = "Product retrieved successfully.";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }

    [HttpGet("GetByCategory/{categoryId:int}")]
    public IActionResult GetByCategory(int categoryId, int limit = 20, int page = 1)
    {
        try
        {
            var products = _dbContext
                .Products
                .Where(p => p.CategoryId == categoryId)
                //.Include(p => p.Category)
                .Skip(limit* (page - 1))
                .Take(limit)
                .ToList();
            
            _paginateDto.CurrentPage = page;
            _paginateDto.Limit = limit;
            _paginateDto.Main = _mapper.Map<List<ProductDto>>(products);
            
            _responseDto.Data = _paginateDto;
            _responseDto.Message = "Products retrieved successfully.";
            
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    public IActionResult Post([FromForm] ProductDto productDto)
    {
        try
        {
            
            var obj = _mapper.Map<Product>(productDto);
            _dbContext.Products.Add(obj);
            _dbContext.SaveChanges();

            if (productDto.Image != null)
            {
                
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                var fileName = Guid.NewGuid() + Path.GetExtension(productDto.Image.FileName);
                var fullPath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    productDto.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
                obj.ImageLocalPath = $"wwwroot/ProductImages/{fileName}";
                obj.ImageUrl = $"{baseUrl}/ProductImages/{fileName}";
            }
            else
            {
                obj.ImageUrl = "https://placehold.co/600x400";
            }
            _dbContext.Products.Update(obj);
            _dbContext.SaveChanges();
            _responseDto.Data = _mapper.Map<ProductDto>(obj);
            _responseDto.Message = "Product successfully added.";
            return Ok(_responseDto);
    
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.Admin)]
    
    [Consumes(MediaTypeNames.Multipart.FormData)]
    public IActionResult Put([FromForm] ProductDto productDto)
    {
        try
        {
            var product = _mapper.Map<Product>(productDto);

            if (productDto.Image != null)
            {
                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    var file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                var fileName = Guid.NewGuid() + Path.GetExtension(productDto.Image.FileName);
                var fullPath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    productDto.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
                product.ImageLocalPath = $"wwwroot/ProductImages/{fileName}";
                product.ImageUrl = $"{baseUrl}/ProductImages/{fileName}";
            }


            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            _responseDto.Data = _mapper.Map<ProductDto>(product);
            /*
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == productDto.Id);
            if (product == null) return NotFound(ResponseHelper.NotFoundResponseDto("Product not found."));
            
            if (!string.IsNullOrEmpty(product.ImageLocalPath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(),product.ImageLocalPath);
                var fileInfo = new FileInfo(oldFilePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                
            }
            
            var obj = _mapper.Map(productDto, product);
            
            //_dbContext.Products.Update(obj);
            
            _dbContext.SaveChanges();
            */
            
            //_responseDto.Data = _mapper.Map<ProductDto>(obj);
            _responseDto.Message = "Product updated successfully.";
            return Ok(_responseDto);

        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public IActionResult Delete(int id)
    {
        try
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(ResponseHelper.NotFoundResponseDto("Product not found."));
            if (!string.IsNullOrEmpty(product.ImageLocalPath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(),product.ImageLocalPath);
                var fileInfo = new FileInfo(oldFilePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                
            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            _responseDto.Message = "Product deleted successfully.";
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }
    

}
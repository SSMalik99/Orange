using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    private IMapper _mapper;
    private readonly PaginateDto _paginateDto;

    public ProductApiController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _responseDto = new ResponseDto();
        _paginateDto = new PaginateDto();
        
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

    [HttpGet("/{id:int}")]
    public IActionResult Get(int id)
    {
        try
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
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
    public IActionResult Post([FromBody] ProductDto productDto)
    {
        try
        {
            
            var obj = _mapper.Map<Product>(productDto);
            _dbContext.Products.Add(obj);
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

    // [HttpPut]
    // [Authorize(Roles = "Admin")]
    // public IActionResult Put([FromBody] ProductDto productDto)
    // {
    //     
    // }
    

}
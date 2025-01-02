using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange.Services.ProductAPI.Data;
using Orange.Services.ProductAPI.Models;
using Orange.Services.ProductAPI.Models.Dto;

namespace Orange.Services.ProductAPI.Controllers;

[Route("api/categories")]
[Authorize]
[ApiController]
public class CategoryApiController: ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ResponseDto _responseDto;
    private readonly PaginateDto _paginateDto;
    
    public CategoryApiController(AppDbContext dbContext, IMapper mapper)
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
            var categories = _dbContext.Categories
                .Skip(limit * (page - 1))
                .Take(limit).ToList();
            _paginateDto.Main = _mapper.Map<List<CategoryDto>>(categories);
            _paginateDto.CurrentPage = page;
            _paginateDto.Limit = limit;
            _responseDto.Data = _paginateDto;
            _responseDto.Message = "Category List successfully retrieved";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
        
    }
    
    [HttpGet("/api/categories/{id:int}")]
    public IActionResult Get(int id)
    {
        try
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound(ResponseHelper.NotFoundResponseDto("Category Not Found"));
            _responseDto.Data = _mapper.Map<CategoryDto>(category);
            _responseDto.Message = "Category successfully retrieved";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
        
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] CategoryDto categoryDto)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            _responseDto.Message = "Category successfully created";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public IActionResult Put([FromBody] CategoryDto categoryDto)
    {
        try
        {
            var existingCategory = _dbContext.Categories.FirstOrDefault(c => c.Id == categoryDto.Id);
            if (existingCategory == null) return NotFound(ResponseHelper.NotFoundResponseDto("Category Not Found"));
            var category = _mapper.Map<Category>(categoryDto);
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            _responseDto.Message = "Category successfully updated";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        try
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound(ResponseHelper.NotFoundResponseDto("Category Not Found"));
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            _responseDto.Message = "Category successfully deleted";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
}
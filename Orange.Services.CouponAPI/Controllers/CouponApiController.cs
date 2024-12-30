using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orange.Services.CouponAPI.Data;
using Orange.Services.CouponAPI.Dto;
using Orange.Services.CouponAPI.Models;

namespace Orange.Services.CouponAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/coupon")]
[ApiController]
public class CouponApiController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ResponseDto _responseDto;
    private IMapper _mapper;

    public CouponApiController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _responseDto = new ResponseDto();
        
    }

    [HttpGet]
    public object Get()
    {
        try
        {
            var coupons = _dbContext.Coupons.ToList();
            _responseDto.Data = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            _responseDto.Message = "All Coupons successfully retrieved.";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.StatusCode = 500;
        }
        
        return _responseDto;
        
    }

    [HttpGet("{id:int}")]
    public ResponseDto Get(int id)
    {
        try
        {
            var coupon = _dbContext.Coupons.FirstOrDefault(obj => obj.Id == id);
            
            if (coupon is null)
            {
                return ResponseHelper.NotFoundResponseDto();
            }
            _responseDto.Data = _mapper.Map<CouponDto>(coupon);
            _responseDto.Message = "Coupon successfully retrieved.";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.StatusCode = 500;
        }
        
        return _responseDto;
        
    }
    
    [HttpGet("GetByCode/{code}")]
    public ResponseDto GetByCode(string code)
    {
        try
        {
            var coupon = _dbContext.Coupons.FirstOrDefault(obj => obj.CouponCode == code);
            
            if (coupon is null)
            {
                return ResponseHelper.NotFoundResponseDto();
            }
            _responseDto.Data = _mapper.Map<CouponDto>(coupon);
            _responseDto.Message = "All Coupons successfully retrieved.";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.StatusCode = 500;
        }
        
        return _responseDto;
        
    }
    
    [HttpPost]
    public ResponseDto Post([FromBody] CouponDto couponDto)
    {
        try
        {
            var obj = _mapper.Map<Coupon>(couponDto);
            _dbContext.Coupons.Add(obj);
            _dbContext.SaveChanges();
            _responseDto.Data = _mapper.Map<CouponDto>(obj);
            _responseDto.Message = "Coupon successfully added.";
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.StatusCode = 500;
        }
        
        return _responseDto;
        
    }

    [HttpPut]
    public ResponseDto Put([FromBody] CouponDto couponDto)
    {
        try
        {
            var obj = _mapper.Map<Coupon>(couponDto);
            _dbContext.Coupons.Update(obj);
            _dbContext.SaveChanges();
            _responseDto.Data = _mapper.Map<CouponDto>(obj);
            _responseDto.Message = "Coupon successfully updated.";
            
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.StatusCode = 500;
        }
        return _responseDto;
    }

    
    [HttpDelete("{id:int}")]
    public ResponseDto Delete(int id)
    {
        try
        {
            var coupon = _dbContext.Coupons.FirstOrDefault(obj => obj.Id == id);
            if (coupon is null) return ResponseHelper.NotFoundResponseDto();
            _dbContext.Coupons.Remove(coupon);
            _dbContext.SaveChanges();
            _responseDto.Message = "Coupon successfully deleted.";
            
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.StatusCode = 500;
        }
        return _responseDto;
    }

    
    
}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange.Services.CouponAPI.Data;
using Orange.Services.CouponAPI.Dto;
using Orange.Services.CouponAPI.Models;
using Orange.Services.CouponAPI.Utility;
using Coupon = Orange.Services.CouponAPI.Models.Coupon;

namespace Orange.Services.CouponAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/coupon")]
[ApiController]
[Authorize]
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
    public IActionResult Get()
    {
        try
        {
            var coupons = _dbContext.Coupons.ToList();
            _responseDto.Data = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            _responseDto.Message = "All Coupons successfully retrieved.";
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
            var coupon = _dbContext.Coupons.FirstOrDefault(obj => obj.Id == id);
            
            if (coupon is null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Coupon does not exist.";
                return NotFound(_responseDto);
            }
            _responseDto.Data = _mapper.Map<CouponDto>(coupon);
            _responseDto.Message = "Coupon successfully retrieved.";
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
        
    }
    
    [HttpGet("GetByCode/{code}")]
    public IActionResult GetByCode(string code)
    {
        try
        {
            var coupon = _dbContext.Coupons.FirstOrDefault(obj => obj.CouponCode == code);
            
            if (coupon is null)
            {
                return NotFound(ResponseHelper.NotFoundResponseDto());
            }
            _responseDto.Data = _mapper.Map<CouponDto>(coupon);
            _responseDto.Message = "All Coupons successfully retrieved.";
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public IActionResult Post([FromBody] CouponDto couponDto)
    {
        try
        {
            
            
            var obj = _mapper.Map<Coupon>(couponDto);
            _dbContext.Coupons.Add(obj);
            _dbContext.SaveChanges();
            
            var stripeCouponOptions = new Stripe.CouponCreateOptions
            {
                AmountOff = (long)(couponDto.CouponAmount * 100),
                Name = couponDto.CouponCode,
                Currency = StaticDetail.PriceCurrency,
                Id = couponDto.CouponCode,
            };
            var stripService = new Stripe.CouponService();
            stripService.Create(stripeCouponOptions);


            _responseDto.Data = _mapper.Map<CouponDto>(obj);
            _responseDto.Message = "Coupon successfully added.";
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().ToString());
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess = false;
            return Ok(_responseDto);
        }
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.Admin)]
    public IActionResult Put([FromBody] CouponDto couponDto)
    {
        try
        {
            var obj = _mapper.Map<Coupon>(couponDto);
            _dbContext.Coupons.Update(obj);
            _dbContext.SaveChanges();
            _responseDto.Data = _mapper.Map<CouponDto>(obj);
            _responseDto.Message = "Coupon successfully updated.";
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            _responseDto.Message = e.Message;
            _responseDto.IsSuccess =false;
            return Ok(_responseDto);
        }
        
    }

    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public IActionResult Delete(int id)
    {
        try
        {
            var coupon = _dbContext.Coupons.FirstOrDefault(obj => obj.Id == id);
            if (coupon is null) return Ok(ResponseHelper.NotFoundResponseDto());
            _dbContext.Coupons.Remove(coupon);
            _dbContext.SaveChanges();
            
            var stripService = new Stripe.CouponService();
            stripService.Delete(coupon.CouponCode);
            
            _responseDto.IsSuccess = true;
            _responseDto.Message = "Coupon successfully deleted.";
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
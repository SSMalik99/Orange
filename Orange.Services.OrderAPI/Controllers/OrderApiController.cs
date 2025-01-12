using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange.Services.OrderAPI.Data;
using Orange.Services.OrderAPI.Models;
using Orange.Services.OrderAPI.Models.Dto;
using Orange.Services.OrderAPI.Models.Enum;
using Orange.Services.OrderAPI.Services.IServices;

namespace Orange.Services.OrderAPI.Controllers;

[ApiController]
[Route("api/order")]
public class OrderApiController : ControllerBase
{
    private readonly ResponseDto _response;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IProductService _productService;
    private readonly ICouponService _couponService;

    public OrderApiController(
        IMapper mapper,
        AppDbContext dbContext,
        IProductService productService,
        ICouponService couponService
        )
    {
        _mapper = mapper;
        _response = new ResponseDto();
        _dbContext = dbContext;
        _productService = productService;
        _couponService = couponService;
        
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(CartDto cartDto)
    {
        try
        {
            var orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.OrderTime = DateTime.Now;
            orderHeaderDto.Status = OrderStatus.Pending;
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartDetails);
            
            var orderCreated = (await _dbContext.OrderHeaders.AddAsync(_mapper.Map<OrderHeader>(orderHeaderDto))).Entity;
            await _dbContext.SaveChangesAsync();
            
            //orderHeaderDto.Id = orderCreated.Id;
            
            _response.Data = _mapper.Map<OrderHeaderDto>(orderCreated);
            return Created("", orderCreated);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
}
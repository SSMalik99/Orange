using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange.Services.OrderAPI.Data;
using Orange.Services.OrderAPI.Models;
using Orange.Services.OrderAPI.Models.Dto;
using Orange.Services.OrderAPI.Models.Enum;
using Orange.Services.OrderAPI.Services.IServices;
using Orange.Services.OrderAPI.Utility;
using Stripe;
using Stripe.Checkout;

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

    
    [Authorize]
    [HttpPost("create")]
    public IActionResult Create(CartDto cartDto)
    {
        try
        {
            var orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.OrderTime = DateTime.Now;
            orderHeaderDto.Status = OrderStatus.Pending;
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartDetails);
            
            var orderCreated = _dbContext.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto));
            _dbContext.SaveChanges();
            
            //orderHeaderDto.Id = orderCreated.Id;
            
            _response.Data = _mapper.Map<OrderHeaderDto>(orderCreated.Entity);
            _response.Message = "Order is created but not paid yet";
            return Ok(_response);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }

    [Authorize]
    [HttpPost("CreatePaymentSession")]
    public async Task<IActionResult> CreatePaymentSession(StripeRequestDto stripeRequestDto)
    {
        try
        {

            var paymentOptions = new SessionCreateOptions
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                LineItems = [],
                Mode = "payment",
            };

            if (stripeRequestDto.OrderHeader.OrderDetails == null)
            {
                return BadRequest(ResponseHelper.GenerateErrorResponse("Payment Session Not Found"));
            }
            
            foreach (var product in stripeRequestDto.OrderHeader.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        Currency = StaticData.PriceCurrency,
                        UnitAmount = (long)product.Price * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.ProductName,
                        }
                    },
                    Quantity = product.Quantity,
                };
                paymentOptions.LineItems.Add(sessionLineItem);
            }

            var paymentService = new SessionService();
            var stripePaymentSession = await paymentService.CreateAsync(paymentOptions);

            stripeRequestDto.StripeSessionUrl = stripePaymentSession.Url;
            var orderHeader = _dbContext.OrderHeaders.FirstOrDefault(oh => oh.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
            
            if (orderHeader == null)
            {
                return BadRequest(ResponseHelper.GenerateErrorResponse("Invalid order properties, please try again!"));
            }

            orderHeader.StripeSessionId = stripePaymentSession.Id;
            await _dbContext.SaveChangesAsync();
            _response.Data = stripeRequestDto;
            return Ok(_response);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
}
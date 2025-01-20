using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.JsonWebTokens;
using Orange.MessageBus;
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
    private readonly IMessageBus _messageBus;

    public OrderApiController(
        IMapper mapper,
        AppDbContext dbContext,
        IProductService productService,
        ICouponService couponService,
        IMessageBus messageBus
        )
    {
        _mapper = mapper;
        _response = new ResponseDto();
        _dbContext = dbContext;
        _productService = productService;
        _couponService = couponService;
        _messageBus = messageBus;
    }


    [HttpGet("GetUserOrders")]
    [Authorize]
    public IActionResult GetUserOrders(string? userId, int limit = 10, int page = 1)
    {
        Console.WriteLine("working");
        try
        {
            // var authorizeUserId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            // Console.WriteLine($"authorizeUserId: {authorizeUserId}");
            // Console.WriteLine($"authorizeUserId: {authorizeUserId}");
            
            // Redirect user when user role is not admin
            if (!User.IsInRole(StaticData.RoleAdmin))
            {
                if (
                    string.IsNullOrEmpty(userId) 
                    //  authorizeUserId == null ||
                    // authorizeUserId != userId
                    )
                {
                    return Unauthorized();    
                }
                
            }
            
            var query = _dbContext.OrderHeaders
                .Include(oh => oh.OrderDetails)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(u => u.UserId == userId);
            }


            _response.Data = new PaginateDto()
            {
                CurrentPage = page,
                Limit = limit,
                Main = _mapper.Map<List<OrderHeaderDto>>(query.Skip(limit * (page - 1))
                    .Take(limit)
                    .ToList())
            };
            _response.Message = "Orders Retrieved Successfully.";
            
            return Ok(_response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }

    
    
    [HttpGet("/{orderId:guid}")]
    [Authorize]
    public IActionResult Get(Guid orderId)
    {
        try
        {
            var orderHeader = _dbContext.OrderHeaders.Include(oh => oh.OrderDetails).FirstOrDefault(order => order.OrderHeaderId == orderId);
            if (orderHeader == null) return Ok(ResponseHelper.NotFoundResponseDto("Order not found"));
            _response.Data = _mapper.Map<OrderHeaderDto>(orderHeader);
            return Ok(_response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
    
    //Creation and Payment Section for the order
    
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
                Mode = "payment"
            };

            var discountsObj = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions()
                {
                    Coupon = stripeRequestDto.OrderHeader?.CouponCode,
                }
            }; 

            if (stripeRequestDto.OrderHeader?.OrderDetails == null)
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

            if (stripeRequestDto.OrderHeader.Discount > 0)
            {
                paymentOptions.Discounts = discountsObj;
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
    
    
    [Authorize]
    [HttpPost("ValidateStripeSession")]
    public async Task<IActionResult> ValidateStripeSession([FromBody] Guid orderHeaderId)
    {
        try
        {
            var orderHeader = await _dbContext.OrderHeaders.FirstOrDefaultAsync(oh => oh.OrderHeaderId == orderHeaderId);

            
            if (orderHeader == null)
            {
                return BadRequest(ResponseHelper.GenerateErrorResponse("Invalid order, please try again!"));
            }
            
            var stripeService = new SessionService();

            var paymentSession = await stripeService.GetAsync(orderHeader.StripeSessionId);

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentSession.PaymentIntentId);
            
            switch (paymentIntent.Status)
            {
                case "succeeded":
                case "complete":
                    
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = OrderStatus.Approved;
                    await _dbContext.SaveChangesAsync();

                    var rewardDto = new RewardsDto()
                    {
                        UserId = orderHeader.UserId,
                        OrderId = orderHeader.OrderHeaderId.ToString(),
                        RewardPoints = Convert.ToInt32(orderHeader.OrderTotal),
                        Email = orderHeader.Email
                    };
                    
                    _ = _messageBus.PublishMessageAsync(rewardDto, StaticData.OrderCreatedTopicName);
                    _response.Message = "Payment for the order is succeeded";
                    _response.Data = _mapper.Map<OrderHeaderDto>(orderHeader);
                    break;
                case "failed":
                    _response.Message = "Payment for the order is failed";
                    break;
                case "cancelled":
                    _response.Message = "Payment for the order is cancelled";
                    break;
                default:
                    _response.Message = "Payment for the order is failed";
                    break;
            }
            
            return Ok(_response);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
    
    
    
}
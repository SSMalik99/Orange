using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orange.MessageBus;
using Orange.Services.ShoppingCartAPI.Data;
using Orange.Services.ShoppingCartAPI.Models;
using Orange.Services.ShoppingCartAPI.Models.Dto;
using Orange.Services.ShoppingCartAPI.Services.IServices;
using Orange.Services.ShoppingCartAPI.Utility;

namespace Orange.Services.ShoppingCartAPI.Controllers;

[Route("api/cart")]
[ApiController]
[Authorize]
public class CartApiController : ControllerBase
{
    
    private readonly ResponseDto _responseDto = new();
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly ICouponService _couponService;
    private readonly IMessageBus _messageBus;
    private readonly IConfiguration _configuration;

    public CartApiController(
        AppDbContext dbContext,
        IMapper mapper,
        IProductService productService,
        ICouponService couponService,
        IMessageBus messageBus, IConfiguration config)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _productService = productService;
        _couponService = couponService;
        _messageBus = messageBus;
        _configuration = config;
        
    }

    [HttpGet("GetCart/{userId:guid}")]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        try
        {
            
            CartDto cartDto = new()
            {
                CartHeader = _mapper.Map<CartHeaderDto>(
                    await _dbContext.CartHeaders.FirstAsync(u => u.UserId == userId)
                ),

            };
            
            cartDto.CartDetails = _mapper.Map<List<CartDetailsDto>>(
                await _dbContext.CartDetails
                    .Where(cd => cd.CartHeaderId == cartDto.CartHeader.Id).ToListAsync()
                );
            
            var productIds = cartDto.CartDetails
                .Select(cd => cd.ProductId) // Extract ProductId
                .ToList();

            
            var productForCart = await _productService.GetAllProductForCart(productIds);

            if (productForCart.Count == 0)
            {
                return BadRequest(ResponseHelper.GenerateErrorResponse("There are no products associated with this cart"));
            }
            
            foreach (var cartDetail in cartDto.CartDetails)
            {
                cartDetail.Product = productForCart.FirstOrDefault(p => p.Id == cartDetail.ProductId);
                
                if (cartDetail.Product != null)
                    cartDto.CartHeader.CartTotal += (double)(cartDetail.Quantity * cartDetail.Product.Price);
            }

            if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
            {
                var couponDto = await _couponService.GetCouponByCode(cartDto.CartHeader.CouponCode);
                
                if (couponDto != null && cartDto.CartHeader.CartTotal > couponDto.MinAmount)
                {
                    cartDto.CartHeader.CartTotal -= couponDto.CouponAmount;
                    cartDto.CartHeader.Discount = couponDto.CouponAmount;
                }
            }
            _responseDto.Data = cartDto;
            _responseDto.Message = "Cart details retrieved successfully";
            return Ok(_responseDto);

        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
        
    }
    
    [HttpPost("CartUpsert")]
    public async Task<IActionResult> CartUpsert([FromBody] CartDto cartDto)
    {
        try
        {
            
            var cartHeaderFromDb = await _dbContext
                .CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(ch => ch.UserId == cartDto.CartHeader.UserId);
            
            var newCartDetail = cartDto.CartDetails?.First();
            
            if (newCartDetail == null || newCartDetail.ProductId == 0 )
            {
                return BadRequest(ResponseHelper.GenerateErrorResponse("Product Detail could not be found"));
            }
            
            if (cartHeaderFromDb == null)
            {
                // Create CartHeader
                var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                _dbContext.CartHeaders.Add(cartHeader);
                await _dbContext.SaveChangesAsync();
                
                newCartDetail.CartHeaderId = cartHeader.Id;
                _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(newCartDetail));
                _responseDto.Message = "Product added to the cart.";
            }
            else
            {
                var cartDetailsFromDb = _dbContext.CartDetails.AsNoTracking().FirstOrDefault(
                    cd => cd.ProductId == newCartDetail.ProductId
                    && cd.CartHeaderId == cartHeaderFromDb.Id
                    );
                
                
                
                if (cartDetailsFromDb == null)
                {
                    newCartDetail.CartHeaderId = cartHeaderFromDb.Id;
                    _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(newCartDetail));
                    _responseDto.Message = "Product added to the cart.";
                }
                else
                {
                    newCartDetail.Quantity += cartDetailsFromDb.Quantity;
                    newCartDetail.CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    newCartDetail.CartId = cartDetailsFromDb.CartId;
                    _dbContext.CartDetails.Update(_mapper.Map<CartDetails>(newCartDetail));
                    _responseDto.Message = "Product updated in the cart.";
                }
                
            }
            
            await _dbContext.SaveChangesAsync();
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }

    [HttpDelete("CartDelete/{cartDetailId:guid}")]
    public async Task<IActionResult> CartDelete(Guid cartDetailId)
    {
        try
        {
            var cartDetail = await _dbContext.CartDetails.FirstOrDefaultAsync(cd => cd.CartId == cartDetailId);
            if (cartDetail == null)
            {
                return NotFound(ResponseHelper.GenerateErrorResponse("Cart not found"));
            }

            var totalCartItems = await _dbContext.CartDetails.Where(cd => cd.CartHeaderId == cartDetail.CartHeaderId)
                .CountAsync();
            _dbContext.CartDetails.Remove(cartDetail);

            if (totalCartItems == 1)
            {
                var cartHeaderToRemove = await _dbContext.CartHeaders.FirstOrDefaultAsync(hd => hd.Id == cartDetail.CartHeaderId);
                if (cartHeaderToRemove != null) _dbContext.CartHeaders.Remove(cartHeaderToRemove);
            }
            _responseDto.Message = "Product deleted from the cart.";
            await _dbContext.SaveChangesAsync();
            return Ok(_responseDto);
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }

    [HttpPost("ApplyCoupon")]
    public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponDto applyCouponDto)
    {
        try
        {
            
            
            
            _responseDto.Message = "Coupon successfully removed from the cart.";
            
            var cartHeader = _dbContext.CartHeaders.FirstOrDefault(ch => ch.UserId == applyCouponDto.UserId);
            if (cartHeader == null)
            {
                return NotFound(ResponseHelper.GenerateErrorResponse("Cart not found"));
            }
            
            if (!string.IsNullOrEmpty(applyCouponDto.CouponCode))
            {
                var couponDto = await _couponService.GetCouponByCode(applyCouponDto.CouponCode);
                
                if (couponDto == null)
                {
                    return BadRequest(ResponseHelper.GenerateErrorResponse("Coupon code could not be found."));
                }

                var products = _dbContext.CartDetails.Where(cd => cd.CartHeaderId == cartHeader.Id).ToList();
                double totalAmount = 0.0; 
                
                foreach (var product in products)
                {
                    var mainProduct = await _productService.GetProductById(product.ProductId);
                    if (mainProduct == null) continue;
                    totalAmount += (double)(mainProduct.Price * product.Quantity);
                }

                if (totalAmount < couponDto.MinAmount)
                {
                    return BadRequest(ResponseHelper.GenerateErrorResponse("Cart too low to apply coupon."));
                }
                
                    
                    
                _responseDto.Message = "Coupon successfully applied.";
            }
            
            cartHeader.CouponCode = applyCouponDto.CouponCode ?? "";
            _dbContext.CartHeaders.Update(cartHeader);
            await _dbContext.SaveChangesAsync();
            
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
    
    [HttpPost("EmailCart")]
    public async Task<IActionResult> EmailCart([FromBody] CartDto cartDto)
    {
        try
        {
            await _messageBus.PublishMessageAsync(cartDto, StaticData.AzureEmailCartQueueName);
            _responseDto.Message = "Cart will be sent to your email address. Please wait for few seconds before sending again.";
            return Ok(_responseDto);
            
        }
        catch (Exception e)
        {
            return Ok(ResponseHelper.GenerateErrorResponse(e.Message));
        }
    }
    
    
}
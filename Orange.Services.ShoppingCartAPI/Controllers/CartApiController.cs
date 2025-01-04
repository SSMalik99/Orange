using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orange.Services.ShoppingCartAPI.Data;
using Orange.Services.ShoppingCartAPI.Models;
using Orange.Services.ShoppingCartAPI.Models.Dto;

namespace Orange.Services.ShoppingCartAPI.Controllers;

[Route("api/cart")]
[ApiController]
public class CartApiController : ControllerBase
{
    private readonly ResponseDto _responseDto;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    

    public CartApiController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _responseDto = new ResponseDto();
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
            foreach (var cartDetail in cartDto.CartDetails)
            {
                //cartDto.CartHeader.CartTotal += (double)(cartDetail.Quantity * cartDetail.Product.Price);
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
                _dbContext.CartHeaders.Remove(cartHeaderToRemove);
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
    
    
}
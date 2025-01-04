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

    [HttpPost("CartUpsert")]
    
    public async Task<IActionResult> CartUpsert([FromBody] CartDto cartDto)
    {
        try
        {
            var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(ch => ch.UserId == cartDto.CartHeader.UserId);
            var newCartDetail = cartDto.CartDetails.First();
            
            if (cartHeaderFromDb == null)
            {
                // Create CartHeader
                var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                _dbContext.CartHeaders.Add(cartHeader);
                await _dbContext.SaveChangesAsync();
                
                newCartDetail.CartHeaderId = cartHeader.Id;
                _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
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
    
}
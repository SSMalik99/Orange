using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Orange.Services.AuthAPI.Models.Dto;
using Orange.Services.AuthAPI.Service;
using Orange.Services.AuthAPI.Service.IService;

namespace Orange.Services.AuthAPI.Controllers;

[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;
    private ResponseDto _responseDto;

    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
        _responseDto = new ResponseDto();
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
    {
        
        var errorMessage = await _authService.Register(registerDto);
        
        if (string.IsNullOrEmpty(errorMessage))
        {
            _responseDto.Message = "Registered successfully.";
            return Ok(_responseDto);
        }
        
        _responseDto.Message = errorMessage;
        _responseDto.IsSuccess = false;
        
        Console.WriteLine(_responseDto.IsSuccess);
        
        return BadRequest(_responseDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await _authService.Login(model);
        if (loginResponse.User is not null)
        {
            _responseDto.Data = loginResponse;
            _responseDto.Message = loginResponse.Message;
            
            return Ok(_responseDto);
        }
        
        _responseDto.Message = loginResponse.Message;
        _responseDto.IsSuccess = false;
        return BadRequest(_responseDto);
    }

    [HttpPost("assignRole")]
    public async Task<IActionResult> Role([FromBody] AssignRoleReqDto model)
    {
        var roleAssigned = await _authService.AssignRole(model.Email, model.RoleName);
        if (!roleAssigned)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Role assign failed.";
            return BadRequest(_responseDto);
        }
        
        _responseDto.Message = "Role assigned successfully.";
        return Ok(_responseDto);

    }
    
}
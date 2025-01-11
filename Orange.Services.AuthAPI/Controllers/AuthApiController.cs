using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Orange.MessageBus;
using Orange.Services.AuthAPI.Models.Dto;
using Orange.Services.AuthAPI.Service;
using Orange.Services.AuthAPI.Service.IService;
using Orange.Services.AuthAPI.Utility;

namespace Orange.Services.AuthAPI.Controllers;

[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ResponseDto _responseDto;
    private readonly IMessageBus _messageBus;

    public AuthApiController(IAuthService authService, IMessageBus messageBus)
    {
        _authService = authService;
        _responseDto = new ResponseDto();
        _messageBus = messageBus;
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
    {
        
        var errorMessage = await _authService.Register(registerDto);
        
        if (string.IsNullOrEmpty(errorMessage))
        {
            _responseDto.Message = "Registered successfully.";
            
            // Send Email to user via service bus
            await _messageBus.PublishMessageAsync(new
            {
                registerDto.Email,
                registerDto.FirstName,
                registerDto.LastName,
            }, StaticData.AzureRegisterQueueName);
            
            
            return Ok(_responseDto);
        }
        
        _responseDto.Message = errorMessage;
        _responseDto.IsSuccess = false;
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
using Orange.Services.AuthAPI.Models;
using Orange.Services.AuthAPI.Models.Dto;

namespace Orange.Services.AuthAPI.Service.IService;

public interface IAuthService
{
    public Task<string> Register(RegistrationDto registrationDto);
    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    public Task<bool> AssignRole(string email, string role);
    
}
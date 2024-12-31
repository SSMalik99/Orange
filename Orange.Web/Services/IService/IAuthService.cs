using Orange.Web.Models;
using Orange.Web.Models.Auth;

namespace Orange.Web.Services.IService;

public interface IAuthService
{
    Task<ResponseDto> LoginAsync(LoginRequestDto loginDto);
    Task<ResponseDto> RegisterAsync(RegistrationDto registrationDto);
    Task<ResponseDto> AssignRoleAsync(AssignRoleReqDto assignRoleReqDto);
    
}
using Orange.Web.Models;
using Orange.Web.Models.Auth;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Services;

public class AuthService(IBaseService baseService) : IAuthService
{
    
    public async Task<ResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.AuthApiBase+"/api/auth/login/",
            Body = loginDto
        }, withAuth: false);
    }

    public async Task<ResponseDto> RegisterAsync(RegistrationDto registrationDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.AuthApiBase+"/api/auth/register/",
            Body = registrationDto
        }, withAuth: false);
    }

    public async Task<ResponseDto> AssignRoleAsync(AssignRoleReqDto assignRoleReqDto)
    {
        
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.AuthApiBase+"/api/auth/assignRole/",
            Body = assignRoleReqDto
        }, withAuth: false);
    }
}
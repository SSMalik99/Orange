using Orange.Web.Models;

namespace Orange.Web.Services.IService;

public interface IBaseService
{
    Task<ResponseDto> SendAsync(RequestDto requestDto, bool withAuth = true);
}
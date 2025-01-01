
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public TokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        
    }
    public void SetToken(string token)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(SharedDetail.TokenCookie, token);
    }

    public string? GetToken()
    {
        string? token = null;
        var hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(SharedDetail.TokenCookie, out token);
        return hasToken is true ? token : null;

    }

    public void RevokeToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SharedDetail.TokenCookie);
    }
}
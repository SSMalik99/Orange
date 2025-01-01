namespace Orange.Web.Services.IService;

public interface ITokenProvider
{
    void SetToken(string token);
    string? GetToken();
    void RevokeToken();
}
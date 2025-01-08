using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Orange.Services.ShoppingCartAPI.Utility;

public class ApiAuthHttpClientHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public ApiAuthHttpClientHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        Console.WriteLine(token);
        return await base.SendAsync(request, cancellationToken);
    }
    
}
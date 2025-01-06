using System.Text;
using Newtonsoft.Json;

namespace Orange.Services.ShoppingCartAPI.Utility;

public static class StaticData
{
    public static string ProductApiBase { get; set; }
    public static string CouponApiBase { get; set; }
}

public static class ApiCallHelper
{
    public static async Task<HttpResponseMessage> SendRequest(
        HttpClient httpClient,
        string url, 
        HttpMethod httpMethod, 
        object? body, 
        string? jwtToken
        )
    {
        
        HttpRequestMessage request = new();
        request.Headers.Add("Accept","application/json");
        if (jwtToken != null)
        {
            request.Headers.Add("Authorization", jwtToken);    
        }
        
        request.RequestUri = new Uri(url);

        if (body is not null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        request.Method = httpMethod;
        return await httpClient.SendAsync(request);
    }

}
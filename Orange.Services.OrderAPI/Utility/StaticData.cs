using System.Text;
using Newtonsoft.Json;

namespace Orange.Services.OrderAPI.Utility;

public static class StaticData
{
    public static string ProductApiBase { get; set; }
    public static string CouponApiBase { get; set; }
    
    public static string AzureQueueConnectionString { get; set; }
    public static string AzureEmailCartQueueName { get; set; }
    
    public static string RoleAdmin = "ADMIN";
    public static string RoleCustomer = "CUSTOMER";
    public static string RoleEmployee = "EMPLOYEE";
}

public static class ApiCallHelper
{
    public static async Task<HttpResponseMessage> SendRequest(
        HttpClient httpClient,
        string url, 
        HttpMethod httpMethod, 
        object? body
        )
    {
        
        HttpRequestMessage request = new();
        request.Headers.Add("Accept","application/json");
        
        request.RequestUri = new Uri(url);

        if (body is not null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        request.Method = httpMethod;
        return await httpClient.SendAsync(request);
    }

}
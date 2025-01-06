using Newtonsoft.Json;
using Orange.Services.ShoppingCartAPI.Models.Dto;
using Orange.Services.ShoppingCartAPI.Services.IServices;
using Orange.Services.ShoppingCartAPI.Utility;

namespace Orange.Services.ShoppingCartAPI.Services;

public class CouponService : ICouponService
{
    
    private readonly    IHttpClientFactory _httpClientFactory;
    
    public CouponService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        
        
    }
    public async Task<CouponDto?> GetCouponByCode(string couponCode, string? authToken)
    {
        var httpClient = _httpClientFactory.CreateClient("CouponAPI");
            
        var response = await ApiCallHelper.SendRequest(
            httpClient,
            StaticData.CouponApiBase + "/api/coupon/GetByCode/"+couponCode,
            HttpMethod.Get,
            null,
            authToken
        );
        
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
        return responseDto.IsSuccess ? JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Data)) : null;
    }
}
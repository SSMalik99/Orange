using System.Text;
using Newtonsoft.Json;
using Orange.Services.ShoppingCartAPI.Models.Dto;
using Orange.Services.ShoppingCartAPI.Services.IServices;
using Orange.Services.ShoppingCartAPI.Utility;

namespace Orange.Services.ShoppingCartAPI.Services;

public class ProductService : IProductService
{
    private readonly    IHttpClientFactory _httpClientFactory;

    private readonly string _productBaseAPI;
    
    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _productBaseAPI = StaticData.ProductApiBase;
    }
    

    
    public async Task<List<ProductDto>> GetProducts(string? userJwt)
    {
            var httpClient = _httpClientFactory.CreateClient("ProductAPI");
            
            var response = await ApiCallHelper.SendRequest(
                httpClient,
                _productBaseAPI + "/api/products",
                HttpMethod.Get,
                null,
                userJwt
                );
        
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
        return responseDto.IsSuccess ?  JsonConvert
            .DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Data)) 
            : [];
    }
    

    public async Task<List<ProductDto>> GetAllProductForCart(List<int> productIds, string? userJwt)
    {
        var httpClient = _httpClientFactory.CreateClient("ProductAPI");
        var response = await ApiCallHelper.SendRequest(
            httpClient,
            _productBaseAPI + "/api/products/GetProductsWithIds",
            HttpMethod.Post,
            productIds,
            userJwt
        );
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
        return responseDto.IsSuccess ?  JsonConvert
                .DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Data)) 
            : [];
        
    }
}
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
    

    
    public async Task<List<ProductDto>> GetProducts()
    {
            var httpClient = _httpClientFactory.CreateClient("ProductAPI");
            
            var response = await ApiCallHelper.SendRequest(
                httpClient,
                _productBaseAPI + "/api/products",
                HttpMethod.Get,
                null
                );
        
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
        return responseDto.IsSuccess ?  JsonConvert
            .DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Data)) 
            : [];
    }

    public async Task<ProductDto?> GetProductById(int productId)
    {
        var httpClient = _httpClientFactory.CreateClient("ProductAPI");
        var response = await ApiCallHelper.SendRequest(
            httpClient,
            _productBaseAPI + "/api/products/"+productId,
            HttpMethod.Get,
            null
        );
        
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        return responseDto.IsSuccess ? JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Data)) : null;
    }
    

    public async Task<List<ProductDto>> GetAllProductForCart(List<int> productIds)
    {
        var httpClient = _httpClientFactory.CreateClient("ProductAPI");
        var response = await ApiCallHelper.SendRequest(
            httpClient,
            _productBaseAPI + "/api/products/GetProductsWithIds",
            HttpMethod.Post,
            productIds
        );
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
        return responseDto is { IsSuccess: true } ?  JsonConvert
                .DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Data)) 
            : [];
        
    }
}
using Orange.Services.ShoppingCartAPI.Models.Dto;

namespace Orange.Services.ShoppingCartAPI.Services.IServices;

public interface IProductService
{
    Task<List<ProductDto>> GetProducts(string? userJwt);
    Task<List<ProductDto>> GetAllProductForCart(List<int> productIds, string? userJwt);

    Task<ProductDto?> GetProductById(int productId, string? userJwt);
}
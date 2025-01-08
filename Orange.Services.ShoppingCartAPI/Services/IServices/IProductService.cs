using Orange.Services.ShoppingCartAPI.Models.Dto;

namespace Orange.Services.ShoppingCartAPI.Services.IServices;

public interface IProductService
{
    Task<List<ProductDto>> GetProducts();
    Task<List<ProductDto>> GetAllProductForCart(List<int> productIds);

    Task<ProductDto?> GetProductById(int productId);
}
using Orange.Services.OrderAPI.Models.Dto;

namespace Orange.Services.OrderAPI.Services.IServices;

public interface IProductService
{
    Task<List<ProductDto>> GetProducts();
    Task<List<ProductDto>> GetAllProductForCart(List<int> productIds);

    Task<ProductDto?> GetProductById(int productId);
}
using Orange.Web.Models;
using Orange.Web.Models.Product;

namespace Orange.Web.Services.IService;

public interface IProductService
{
    Task<ResponseDto> GetPaginatedProductAsync();
    Task<ResponseDto> GetProductByIdAsync(int id);
    Task<ResponseDto> GetProductByCategoryIdAsync(int categoryId);
    Task<ResponseDto> CreateProductAsync(ProductDto productDto);
    Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
    Task<ResponseDto> DeleteProductAsync(int id);
    
    
    Task<ResponseDto> GetPaginatedCategoryAsync();
    Task<ResponseDto> GetCategoryByIdAsync(int id);
    Task<ResponseDto> CreateCategoryAsync(CategoryDto categoryDto);
    Task<ResponseDto> UpdateCategoryAsync(CategoryDto categoryDto);
    Task<ResponseDto> DeleteCategoryAsync(int id);
}
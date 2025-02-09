using Orange.Web.Models;
using Orange.Web.Models.Product;
using Orange.Web.Services.IService;
using Orange.Web.Utility;

namespace Orange.Web.Services;

public class ProductService(IBaseService baseService) : IProductService
{
    
    public async Task<ResponseDto> GetPaginatedProductAsync()
    {
        return await baseService.SendAsync(new RequestDto()
            {
                ApiType = SharedDetail.ApiType.Get,
                Url = SharedDetail.ProductApiBase+"/api/products",
                
        });
    }

    public async Task<ResponseDto> GetProductByIdAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.ProductApiBase+"/api/products/"+id,
                
        });
    }

    public async Task<ResponseDto> GetProductByCategoryIdAsync(int categoryId)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.ProductApiBase+"/api/products/GetByCategory/"+categoryId,
        });
    }

    public async Task<ResponseDto> CreateProductAsync(ProductDto productDto)
    {
        Console.WriteLine(productDto.Image);
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.ProductApiBase+"/api/products",
            ContentType = SharedDetail.ContactType.MultipartFormData,
            Body = productDto,
        });
    }

    public async Task<ResponseDto> UpdateProductAsync(ProductDto productDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Put,
            Url = SharedDetail.ProductApiBase+"/api/products",
            ContentType = SharedDetail.ContactType.MultipartFormData,
            Body = productDto,
        });
    }

    public async Task<ResponseDto> DeleteProductAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Delete,
            Url = SharedDetail.ProductApiBase+"/api/products/"+id,
        });
    }

    public async Task<ResponseDto> GetPaginatedCategoryAsync()
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.ProductApiBase+"/api/categories/",
        });
    }

    public async Task<ResponseDto> GetCategoryByIdAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Get,
            Url = SharedDetail.ProductApiBase+"/api/categories/"+id,
        });
    }

    public async Task<ResponseDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Post,
            Url = SharedDetail.ProductApiBase+"/api/categories/",
            Body = categoryDto,
        });
    }

    public async Task<ResponseDto> UpdateCategoryAsync(CategoryDto categoryDto)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Put,
            Url = SharedDetail.ProductApiBase+"/api/categories/",
            Body = categoryDto,
        });
    }

    public async Task<ResponseDto> DeleteCategoryAsync(int id)
    {
        return await baseService.SendAsync(new RequestDto()
        {
            ApiType = SharedDetail.ApiType.Delete,
            Url = SharedDetail.ProductApiBase+"/api/categories/"+id,
        });
    }
}
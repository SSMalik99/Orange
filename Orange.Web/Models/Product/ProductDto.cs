using Orange.Web.Utility;

namespace Orange.Web.Models.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    [ValidExtensions([".jpg", ".jpeg", ".png", "ico"])]
    [ValidFileSize(1)]
    public IFormFile? Image { get; set; }
    public int CategoryId { get; set; }
    
    public CategoryDto? Category { get; set; }

    public int Count { get; set; } = 1;

}
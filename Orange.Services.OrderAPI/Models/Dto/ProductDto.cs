namespace Orange.Services.OrderAPI.Models.Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public int CategoryId { get; set; }
    
    public CategoryDto? Category { get; set; }
    
}
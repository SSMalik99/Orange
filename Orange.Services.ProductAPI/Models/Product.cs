using System.ComponentModel.DataAnnotations;

namespace Orange.Services.ProductAPI.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(1,100000)]public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    
    public Category? Category { get; set; }
}
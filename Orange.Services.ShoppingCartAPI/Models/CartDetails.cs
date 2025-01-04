using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orange.Services.ShoppingCartAPI.Models.Dto;

namespace Orange.Services.ShoppingCartAPI.Models;

public class CartDetails
{
    [Key]
    public Guid CartId { get; set; }
    public Guid CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")]
    public CartHeader CartHeader { get; set; }
    public int ProductId { get; set; }
    
    [NotMapped]
    public ProductDto Product { get; set; }
    
    public int Quantity { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orange.Services.OrderAPI.Models.Dto;

namespace Orange.Services.OrderAPI.Models;

public class OrderDetails
{
    [Key]
    public Guid Id { get; set; }
    public Guid OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }
    
    public int ProductId { get; set; }
    
    [NotMapped]
    public ProductDto? Product { get; set; }
    
    public int Quantity { get; set; }
    
    public string ProductName { get; set; }
    public double Price { get; set; }
}
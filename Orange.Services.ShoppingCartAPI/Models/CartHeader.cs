using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orange.Services.ShoppingCartAPI.Models;

public class CartHeader
{
    
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? CouponCode { get; set; }
    
    [NotMapped]
    public double Discount { get; set; }
    [NotMapped]
    public double CartTotal { get; set; }
    
}
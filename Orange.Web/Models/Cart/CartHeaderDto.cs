using System.ComponentModel.DataAnnotations;

namespace Orange.Web.Models.Cart;

public class CartHeaderDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double CartTotal { get; set; }
    
    [Required]public string? FirstName { get; set; } 
    [Required]public string? LastName { get; set; } 
    [Required]public string? Email { get; set; } 
    [Required]public string? PhoneNumber { get; set; } 
}